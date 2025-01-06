using AlphaX.CalcEngine;
using System;
using System.Collections.Generic;
using System.Data;

namespace AlphaX.Sheets.Data
{
    public class WorkSheetDataStore : IDisposable
    {
        private WorkSheet _workSheet;
        private Dictionary<Cell, object> _unboundValues;
        private AlphaXDataCollection _collection;

        public object ActualDataSource { get; private set; }
        public bool IsValid { get; private set; }

        internal WorkSheetDataStore(WorkSheet worksheet)
        {
            _workSheet = worksheet;
            InitializeUnboundDataStore();
        }

        internal WorkSheetDataStore(WorkSheet worksheet, object dataSource) : this(worksheet)
        {
            _workSheet = worksheet;
            InitializeBoundDataStore(dataSource);
        }

        private void InitializeUnboundDataStore()
        {
            _unboundValues = new Dictionary<Cell, object>();
            IsValid = true;
        }

        private void InitializeBoundDataStore(object dataSource)
        {
            IsValid = false;
            _collection = new AlphaXDataCollection(dataSource);

            if(_collection.DataSourceType != DataSourceType.NotSupported)
            {
                IsValid = true;
                _workSheet.RowCount = _collection.Count;
            }

            if (IsValid)
                ActualDataSource = dataSource;
        }

        public object GetValue(int row, int column)
        {
            var cell = _workSheet.Cells.GetCell(row, column, false);

            if (cell != null && _unboundValues.ContainsKey(cell))
            {
                return _unboundValues[cell];
            }
            else if (cell != null && cell.Formula != null)
            {
                var result = _workSheet.WorkBook.CalcEngine.GetValue(_workSheet.Name, row, column) as CalcValue;

                if (result.Kind == CalcValueKind.Error)
                    return (result.Value as CalcError).Error;

                return result.Value;
            }
            else if (IsValid && ActualDataSource != null && row <= _collection.Count - 1)
            {
                var sheetColumn = _workSheet.Columns.GetItem(column, false);
                var dataMap = cell?.DataMap != null ? cell.DataMap : sheetColumn?.DataMap;
                if (dataMap != null && dataMap is PropertyDataMap propertyDataMap
                    && !string.IsNullOrEmpty(propertyDataMap.PropertyName))
                {
                    var item = _collection.GetItemAt(row);
                    return _collection.GetPropertyInfo(propertyDataMap.PropertyName).GetValue(item);
                }
                else if (dataMap != null && dataMap is DataColumnDataMap dataColumnMap
                    && !string.IsNullOrEmpty(dataColumnMap.ColumnName))
                {
                    var item = _collection.GetItemAt(row) as DataRow;
                    return item[dataColumnMap.ColumnName];
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
            var oldValue = GetValue(row, column);

            if (oldValue == value)
                return;

            var cell = _workSheet.Cells[row, column];

            if (cell.Formula != null)
                cell.Formula = null;

            var sheetColumn = _workSheet.Columns.GetItem(column, false);
            var dataMap = cell.DataMap != null ? cell.DataMap : sheetColumn?.DataMap;

            if (_collection != null && row >= _collection.Count)
                dataMap = null;

            if (dataMap != null)
            {
                if (dataMap is PropertyDataMap propertyDataMap)
                    SetPropertyValue(row, column, propertyDataMap, value);
                else if (dataMap is DataColumnDataMap dataColumnMap)
                    SetDataTableCellValue(row, column, dataColumnMap, value);
            }
            else
            {
                SetUnboundCellValue(cell, value);
            }

            _workSheet.WorkBook.DataProvider.RaiseValueChanged(new ValueChangedEventArgs()
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
            var propertyInfo = _collection.GetPropertyInfo(map.PropertyName);

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
            var item = _collection.GetItemAt(row) as DataRow;
            var type = item.Table.Columns[map.ColumnName].DataType;

            if (type != value.GetType())
                return;

            item.BeginEdit();
            item[map.ColumnName] = value;
            item.EndEdit();
        }

        public void Dispose()
        {
            _unboundValues.Clear();
            _workSheet = null;
            _unboundValues = null;
            _collection = null;
            ActualDataSource = null;
        }
    }
}
