using AlphaX.Sheets.Data;
using AlphaX.Sheets.Filtering;
using AlphaX.Sheets.Utils;
using System;

namespace AlphaX.Sheets
{
    public class WorkSheet : IWorkSheet
    {
        public event EventHandler<CellChangedEventArgs> CellChanged;
        public event EventHandler<RangeChangedEventArgs> RangeSorted;
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
        }

        public void SortRange(CellRange range, bool ascending)
        {
            ((Cells)_cells[range.TopRow, range.LeftColumn, range.RowCount, range.ColumnCount]).Sort(ascending);
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

        public void CalculateAll()
        {

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
            if (WorkBook.UpdateProvider != null && !WorkBook.UpdateProvider.SuspendUpdates)
                WorkBook.UpdateProvider.CellChanged(this, args.Row, args.Column, args.OldValue, args.NewValue, args.Action, args.ChangeType);

            CellChanged?.Invoke(this, args);
        }

        internal void OnRangeChanged(RangeChangedEventArgs args)
        {
            if (WorkBook.UpdateProvider != null && !WorkBook.UpdateProvider.SuspendUpdates)
                WorkBook.UpdateProvider.RangeChanged(this, args.CellRange, args.Action, args.ChangeType);

            RangeSorted?.Invoke(this, args);
        }

        internal void OnRowsChanged(RowChangedEventArgs args)
        {
            if (WorkBook.UpdateProvider != null && !WorkBook.UpdateProvider.SuspendUpdates)
                WorkBook.UpdateProvider.RowsChanged(this, args.Index, args.Count, args.Action, args.ChangeType);

            RowsChanged?.Invoke(this, args);
        }

        internal void OnColumnsChanged(ColumnChangedEventArgs args)
        {
            if (WorkBook.UpdateProvider != null && !WorkBook.UpdateProvider.SuspendUpdates)
                WorkBook.UpdateProvider.ColumnsChanged(this, args.Index, args.Count, args.Action, args.ChangeType);

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
                        var sheetColumn = _columns.GetItem(col, false);
                        var sheetRow = _rows.GetItem(row, false);

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
    }
}
