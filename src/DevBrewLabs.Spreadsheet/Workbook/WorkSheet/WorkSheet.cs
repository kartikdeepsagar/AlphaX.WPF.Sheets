using DevBrewLabs.Spreadsheet.Core;
using DevBrewLabs.Spreadsheet.Data;
using DevBrewLabs.Spreadsheet.Filtering;
using DevBrewLabs.Spreadsheet.Utils;
using System;
using System.Collections.Generic;
using static DevBrewLabs.Spreadsheet.Cells;

namespace DevBrewLabs.Spreadsheet
{
    public class WorkSheet : IWorkSheet
    {
        public event EventHandler<CellChangedEventArgs> CellChanged;
        public event EventHandler<RangeChangedEventArgs> RangeChanged;
        public event EventHandler<RowChangedEventArgs> RowsChanged;
        public event EventHandler<ColumnChangedEventArgs> ColumnsChanged;
        private string _name;
        private WorkBook _workBook;
        private Cells _cells;
        private Rows _rows;
        private Columns _columns;
        private RowHeaders _rowHeaders;
        private ColumnHeaders _columnHeaders;
        private TopLeft _topLeft;
        private FilterProvider _filterProvider;
        private WorkSheetDataStore _dataStore;
        private Dictionary<int, ColumnData> _columnStore;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    ((WorkSheets)_workBook.WorkSheets).VerifySheetName(value);
                    _name = value;
                }
            }
        }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int DefaultRowHeight { get; set; }
        public int DefaultColumnWidth { get; set; }
        public bool AllowMultiLineText { get; set; }
        public object DataSource
        {
            get
            {
                if (_dataStore.IsValid && _dataStore.ActualDataSource != null)
                    return _dataStore.ActualDataSource;

                return null;
            }
            set
            {
                InitializeDataStore(value);
            }
        }

        public IRows Rows => _rows;
        public IColumns Columns => _columns;
        public IRange Cells => _cells;
        public IRowHeaders RowHeaders => _rowHeaders;
        public IColumnHeaders ColumnHeaders => _columnHeaders;
        public IFilterProvider FilterProvider => _filterProvider;
        public ITopLeft TopLeft => _topLeft;
        public IDataStore DataStore => _dataStore;
        public IWorkBook WorkBook => _workBook;

        internal WorkSheet(WorkBook book, string name)
        {
            _workBook = book;
            Name = name;
            DefaultRowHeight = 22;
            DefaultColumnWidth = 70;
            AllowMultiLineText = true;
            _rows = new Rows(this);
            _columns = new Columns(this);
            _topLeft = new TopLeft(this);
            _rowHeaders = new RowHeaders(this);
            _columnHeaders = new ColumnHeaders(this);
            _cells = new Cells(this);
            RowCount = ColumnCount = 500;
            _dataStore = new WorkSheetDataStore(this);
            _filterProvider = new FilterProvider(this);
            _columnStore = new Dictionary<int, ColumnData>();
        }

        public void SortRange(CellRange range, bool ascending)
        {
            SortImpl(range, ascending, range.LeftColumn);
        }

        public void Sort(bool ascending, int keyColumn, bool hasHeader = false, bool sortColumnOnly = false)
        {
            SortImpl(new CellRange(
                _cells.Row, 
                _cells.Column, 
                _cells.RowCount, 
                _cells.ColumnCount), 
                ascending, keyColumn, hasHeader, sortColumnOnly);
        }


        public object[,] GetData(CellRange range)
        {
            return GetData(range.TopRow, range.LeftColumn, range.RowCount, range.ColumnCount);
        }

        public object[,] GetData(int row, int column, int rowCount, int columnCount)
        {
            object[,] data = new object[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    data[i, j] = DataStore.GetValue(i + row, j + column);
                }
            }
            return data;
        }

        public void Load(object[,] data, int startRow = 0, int startCol = 0)
        {
            if (data == null)
                return;

            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            if (rows == 0 || cols == 0)
                return;

            for (int c = 0; c < cols; c++)
            {
                int colIndex = startCol + c;
                var colData = GetColumnData(colIndex, true);

                for (int r = 0; r < rows; r++)
                {
                    int rowIndex = startRow + r;
                    object val = data[r, c];
                    colData.SetValue(rowIndex, val);

                    DataStore.SetValue(rowIndex, colIndex, val);
                }
            }

            OnRangeChanged(new RangeChangedEventArgs(
                     SheetRegion.Cells,
                     new CellRange(startRow, startCol, rows, cols),
                      RangeChangeType.Value));
        }

        internal ColumnData GetColumnData(int column, bool createIfNotExists = true)
        {
            if (_columnStore.TryGetValue(column, out var colData))
                return colData;

            if (createIfNotExists)
            {
                colData = new ColumnData(column);
                _columnStore[column] = colData;
                return colData;
            }

            return null;
        }

        internal void ClearColumnCells(int column)
        {
            var colData = GetColumnData(column, false);
            colData?.Clear();
            _cells.ClearColumnCells(column);
        }

        private void SortImpl(CellRange range, bool ascending, int keyColumn, bool hasHeader = false, bool sortColumnOnly = false)
        {
            int startRow = range.TopRow;
            int totalRows = RowCount;
            int startCol = range.LeftColumn;

            if(keyColumn < range.LeftColumn || keyColumn > range.RightColumn)
            {
                keyColumn = range.LeftColumn;
            }

            int totalCols = ColumnCount;

            if (totalRows <= 1)
                return;

            int sortStartRow = hasHeader ? startRow + 1 : startRow;
            int sortRowCount = hasHeader ? totalRows - 1 : totalRows;

            if (sortRowCount <= 1)
                return;

            int targetStartCol = sortColumnOnly ? keyColumn : startCol;
            int targetEndCol = sortColumnOnly ? keyColumn : (startCol + totalCols - 1);

            List<RowSnapshot> snapshots = new List<RowSnapshot>(sortRowCount);

            for (int r = sortStartRow; r < sortStartRow + sortRowCount; r++)
            {
                object keyVal = DataStore.GetValue(r, keyColumn);

                if (keyVal == null)
                    keyVal = _cells.GetCell(r, keyColumn, false)?.Value;

                var snapshot = new RowSnapshot(r, keyVal);

                for (int c = targetStartCol; c <= targetEndCol; c++)
                {
                    var colData = GetColumnData(c, false);
                    if (colData != null)
                    {
                        var cellData = colData.GetCellData(r);
                        snapshot.Data[c] = cellData;
                    }
                }

                snapshots.Add(snapshot);
            }

            snapshots.Sort(new NaturalSortComparer(ascending));

            for (int i = 0; i < snapshots.Count; i++)
            {
                int targetRow = sortStartRow + i;
                var snapshot = snapshots[i];

                for (int c = targetStartCol; c <= targetEndCol; c++)
                {
                    var colData = GetColumnData(c, true);
                    if (snapshot.Data.TryGetValue(c, out var cellData))
                    {
                        colData.SetCellData(targetRow, cellData);

                        if (DataSource != null)
                            DataStore.SetValue(targetRow, c, cellData.Value);
                    }
                    else
                    {
                        colData.ClearRow(targetRow);
                        if (DataSource != null)
                            DataStore.SetValue(targetRow, c, null);
                    }
                }
            }

            OnRangeChanged(new RangeChangedEventArgs(
                 SheetRegion.Cells,
                new CellRange(sortStartRow, targetStartCol, sortRowCount, targetEndCol - targetStartCol + 1),
                RangeChangeType.Sort
            ));
        }

        private void InitializeDataStore(object dataSource)
        {
            if(dataSource == null && DataStore != null)
            {
                _dataStore.Dispose();
                _dataStore = null;
                return;
            }

            if(DataStore != null)
            {
                _dataStore.Dispose();
                _dataStore = null;
            }

            _dataStore = new WorkSheetDataStore(this, dataSource);          
        }

        internal void OnCellChanged(CellChangedEventArgs args)
        {
            args.WorkSheet = this;
            if (_workBook.UpdateProvider != null && !_workBook.UpdateProvider.SuspendUpdates)
                _workBook.UpdateProvider.CellChanged(this, args.Row, args.Column, args.OldValue, args.NewValue, args.Region, args.ChangeType);

            CellChanged?.Invoke(this, args);
        }

        internal void OnRangeChanged(RangeChangedEventArgs args)
        {
            args.WorkSheet = this;
            if (_workBook.UpdateProvider != null && !_workBook.UpdateProvider.SuspendUpdates)
                _workBook.UpdateProvider.RangeChanged(this, args.Range, args.Region, args.ChangeType);

            RangeChanged?.Invoke(this, args);
        }

        internal void OnRowsChanged(RowChangedEventArgs args)
        {
            args.WorkSheet = this;
            if (_workBook.UpdateProvider != null && !_workBook.UpdateProvider.SuspendUpdates)
                _workBook.UpdateProvider.RowsChanged(this, args.Index, args.Count, args.Region, args.ChangeType);

            RowsChanged?.Invoke(this, args);
        }

        internal void OnColumnsChanged(ColumnChangedEventArgs args)
        {
            args.WorkSheet = this;

            if (_workBook.UpdateProvider != null && !_workBook.UpdateProvider.SuspendUpdates)
                _workBook.UpdateProvider.ColumnsChanged(this, args.Index, args.Count, args.Region, args.ChangeType);

            ColumnsChanged?.Invoke(this, args);
        }

        public void Dispose()
        {
            _dataStore.Dispose();
            _dataStore = null;
            DataSource = null;
            _rows.Dispose();
            _columns.Dispose();
            _cells.Dispose();
            _rowHeaders.Dispose();
            _columnHeaders.Dispose();
            _rows = null;
            _columns = null;
            _cells = null;
            _rowHeaders = null;
            _columnHeaders = null;
            _topLeft = null;
            _filterProvider = null;
            _workBook = null;
        }

        public void Clear(WorkSheetClearMode mode)
        {
            switch(mode)
            {
                case WorkSheetClearMode.Data:
                    foreach (var col in _columnStore.Values)
                    {
                        col.Clear();
                    }
                    _columnStore.Clear();
                    _cells.ClearCellStore();
                    break;
            }
        }

        public bool ContainsRange(int row, int column, int rowCount, int columnCount)
        {
            return row >= 0 && column >= 0 &&
                row < RowCount && column < ColumnCount &&
                row + rowCount - 1 < RowCount && 
                column + columnCount - 1 < ColumnCount;
        }

        public void AutoSizeRow(int row)
        {
            if (row < 0 || row >= RowCount)
                return;

            int maxRequiredHeight = DefaultRowHeight;

            if (AllowMultiLineText)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    var value = DataStore.GetValue(row, col);
                    if (value == null)
                        continue;

                    string text = value.ToString();
                    if (string.IsNullOrEmpty(text))
                        continue;

                    string[] lines = TextUtils.GetLines(text);
                    if (lines.Length > 1)
                    {
                        var cell = _cells.GetCell(row, col, false);
                        var sheetColumn = _columns.GetItem(col);
                        var sheetRow = _rows.GetItem(row);

                        double fontSize = 14;
                        string styleName = cell?.StyleName ?? sheetColumn?.StyleName ?? sheetRow?.StyleName;
                        if (!string.IsNullOrEmpty(styleName))
                        {
                            var namedStyle = _workBook?.GetNamedStyle(styleName);
                            if (namedStyle != null)
                                fontSize = namedStyle.FontSize;
                        }

                        double fontLineHeight = Math.Max(fontSize + 2, Math.Round(fontSize * 1.3));
                        int cellRequiredHeight = (int)Math.Ceiling(DefaultRowHeight + (lines.Length - 1) * fontLineHeight);
                        if (cellRequiredHeight > maxRequiredHeight)
                        {
                            maxRequiredHeight = cellRequiredHeight;
                        }
                    }
                }
            }

            int currentHeight = Rows.GetRowHeight(row);
            if (currentHeight != maxRequiredHeight)
            {
                Rows[row].Height = maxRequiredHeight;
            }
        }

        #region private
        internal struct RowSnapshot
        {
            public int OriginalRow { get; }
            public object KeyValue { get; }
            public Dictionary<int, CellData> Data { get; }

            public RowSnapshot(int originalRow, object keyValue)
            {
                OriginalRow = originalRow;
                KeyValue = keyValue;
                Data = new Dictionary<int, CellData>();
            }
        }
        #endregion
    }
}
