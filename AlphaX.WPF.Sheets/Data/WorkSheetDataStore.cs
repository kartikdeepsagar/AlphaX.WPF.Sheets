using GrapeCity.CalcEngine;
using GrapeCity.Sheets.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace GrapeCity.WPF.Sheets.Data
{
    internal class WorkSheetDataStore : IDisposable
    {
        private ListCollectionView _collection;
        private WorkSheet _workSheet;
        private WorkBook _workBook;
        private Cells _cells;
        private Columns _columns;
        private Rows _rows;
        private Dictionary<Cell, object> _unboundValues;
        private Dictionary<string, PropertyInfo> _itemPropertyInfo;

        public object ActualDataSource { get; private set; }
        public bool IsValid { get; private set; }

        public WorkSheetDataStore(WorkSheet worksheet)
        {
            Initialize(worksheet);
            InitializeUnboundDataStore();
        }

        public WorkSheetDataStore(WorkSheet worksheet, object dataSource) : this(worksheet)
        {
            Initialize(worksheet);
            InitializeBoundDataStore(dataSource);
        }

        private void Initialize(WorkSheet worksheet)
        {
            _workSheet = worksheet;
            _workBook = worksheet.WorkBook.As<WorkBook>();
            _cells = _workSheet.Cells.As<Cells>();
            _columns = _workSheet.Columns.As<Columns>();
            _rows = _workSheet.Rows.As<Rows>();
        }

        private void InitializeUnboundDataStore()
        {
            _unboundValues = new Dictionary<Cell, object>();
            IsValid = true;
        }

        private void InitializeBoundDataStore(object dataSource)
        {
            IsValid = true;

            if (dataSource is IList source)
            {
                _collection = new ListCollectionView(source);

                _itemPropertyInfo = new Dictionary<string, PropertyInfo>();
                var itemType = _collection.SourceCollection.GetEnumerator().GetType().GetGenericArguments()[0];
                foreach (var property in _collection.ItemProperties)
                {
                    _itemPropertyInfo.Add(property.Name, itemType.GetProperty(property.Name));
                }
            }
            else if (dataSource is DataTable tableSource)
            {
                _collection = new ListCollectionView(tableSource.AsDataView());
            }
            else if (dataSource is IEnumerable enumerableSource)
            {
                var listSource = enumerableSource.OfType<dynamic>().ToList();
                _collection = new ListCollectionView(listSource);
                _workSheet.RowCount = _collection.Count;
            }
            else
            {
                IsValid = false;
            }

            if (IsValid)
                ActualDataSource = dataSource;
        }

        public IEnumerable<KeyValuePair<int, object>> GetCellValues(int column)
        {
            var sheetColumn = _columns.GetItem(column, false);

            if (sheetColumn == null)
                return _cells.GetCellValues(column);

            if (sheetColumn != null && sheetColumn.DataMap != null)
            {
                var items = new List<KeyValuePair<int, object>>();
                for (int row = 0; row < _collection.Count; row++)
                {
                    var value = GetValue(row, column);

                    if (value != null)
                        items.Add(new KeyValuePair<int, object>(row, value));
                }
                return items;
            }

            return null;
        }

        public object GetValue(int row, int column)
        {
            var cell = _cells.GetCell(row, column, false);

            if (cell != null && _unboundValues.ContainsKey(cell))
            {
                return _unboundValues[cell];
            }
            else if (cell != null && cell.Formula != null)
            {
                var result = _workBook.CalcEngine.GetValue(_workSheet.Name, row, column) as CalcValue;

                if (result.Kind == CalcValueKind.Error)
                    return (result.Value as CalcError).Error;

                return result.Value;
            }
            else if (IsValid && ActualDataSource != null && row <= _collection.Count - 1)
            {
                var sheetColumn = _columns.GetItem(column, false);
                var dataMap = cell?.DataMap != null ? cell.DataMap : sheetColumn?.DataMap;
                if (dataMap != null && dataMap is PropertyDataMap propertyDataMap
                    && !string.IsNullOrEmpty(propertyDataMap.PropertyName))
                {
                    var item = _collection.GetItemAt(row);
                    return _itemPropertyInfo[propertyDataMap.PropertyName].GetValue(item);
                }
                else if (dataMap != null && dataMap is DataColumnDataMap dataColumnMap
                    && !string.IsNullOrEmpty(dataColumnMap.ColumnName))
                {
                    var item = _collection.GetItemAt(row) as DataRowView;
                    return item.Row[dataColumnMap.ColumnName];
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void SetValue(int row, int column, object value)
        {
            var cell = _workSheet.Cells[row, column].As<Cell>();

            if (cell.Formula != null)
                cell.Formula = null;

            var sheetColumn = _columns.GetItem(column, false);
            var dataMap = cell.DataMap != null ? cell.DataMap : sheetColumn?.DataMap;

            if (_collection != null && row >= _collection.Count)
                dataMap = null;

            var oldValue = GetValue(row, column);

            if (oldValue == value)
                return;

            if (dataMap != null && dataMap is PropertyDataMap propertyDataMap)
            {
                SetPropertyValue(row, column, propertyDataMap, value);
            }
            else if (dataMap != null && dataMap is DataColumnDataMap dataColumnMap)
            {
                SetDataTableCellValue(row, column, dataColumnMap, value);
            }
            else
            {
                SetUnboundCellValue(cell, value);
            }

            _workBook.DataProvider.RaiseValueChanged(new ValueChangedEventArgs()
            {
                Row = row,
                Column = column,
                SheetName = _workSheet.Name,
                OldValue = oldValue,
                NewValue = value
            });
        }

        /// <summary>
        /// Sets the unbound cell value
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        private void SetUnboundCellValue(Cell cell, object value)
        {
            if (_unboundValues.ContainsKey(cell))
            {
                if (value == null)
                    _unboundValues.Remove(cell);
                else
                    _unboundValues[cell] = value;
            }
            else if (value != null)
            {
                _unboundValues.Add(cell, value);
            }
        }

        /// <summary>
        /// Sets the value of cell bound to an object
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="map"></param>
        /// <param name="value"></param>
        private void SetPropertyValue(int row, int column, PropertyDataMap map, object value)
        {
            var item = _collection.GetItemAt(row);
            var propertyInfo = _itemPropertyInfo[map.PropertyName];

            if (propertyInfo.PropertyType != value.GetType() || propertyInfo.SetMethod == null)
                return;

            propertyInfo.SetValue(item, value);
        }

        /// <summary>
        /// Sets the value of cell bound to DataTable.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="map"></param>
        /// <param name="value"></param>
        private void SetDataTableCellValue(int row, int column, DataColumnDataMap map, object value)
        {
            var item = _collection.GetItemAt(row) as DataRowView;
            var type = item.DataView.Table.Columns[map.ColumnName].DataType;

            if (type != value.GetType())
                return;

            item.BeginEdit();
            item[map.ColumnName] = value;
            item.EndEdit();
        }

        public void Dispose()
        {
            _unboundValues.Clear();
            _columns = null;
            _cells = null;
            _rows = null;
            _workSheet = null;
            _itemPropertyInfo = null;
            _unboundValues = null;
            _collection = null;
            ActualDataSource = null;
        }
    }
}
