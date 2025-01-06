using AlphaX.Sheets.Data;
using AlphaX.Sheets.Filtering;
using System;

namespace AlphaX.Sheets
{
    public interface IUpdateProvider
    {
        bool SuspendUpdates { get; set; }
        void RangeChanged(WorkSheet worksheet, CellRange range, SheetAction action, ChangeType changeType);
        void CellChanged(WorkSheet worksheet, int row, int column, object oldValue, object newValue, SheetAction action, ChangeType changeType);
        void RowsChanged(WorkSheet worksheet, int index, int count, SheetAction action, ChangeType changeType);
        void ColumnsChanged(WorkSheet worksheet, int index, int count, SheetAction action, ChangeType changeType);
    }

    public class WorkSheet : IWorkSheet
    {
        public event EventHandler<CellChangedEventArgs> CellChanged;
        public event EventHandler<RangeChangedEventArgs> RangeSorted;
        public event EventHandler<RowChangedEventArgs> RowsChanged;
        public event EventHandler<ColumnChangedEventArgs> ColumnsChanged;

        public WorkSheetDataStore DataStore { get; private set; }
        public string Name { get; set; }
        public WorkBook WorkBook { get; private set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int DefaultRowHeight { get; set; }
        public int DefaultColumnWidth { get; set; }
        public Rows Rows { get; private set; }
        public Columns Columns { get; private set; }
        public Cells Cells { get; private set; }
        public object DataSource
        {
            get
            {
                if (DataStore.IsValid && DataStore.ActualDataSource != null)
                    return DataStore.ActualDataSource;

                return null;
            }
            set
            {
                InitializeDataStore(value);
            }
        }
        public RowHeaders RowHeaders { get; private set; }
        public ColumnHeaders ColumnHeaders { get; private set; }
        public FilterProvider FilterProvider { get; private set; }
        public TopLeft TopLeft { get; private set; }

        internal WorkSheet(WorkBook book, string name)
        {
            Name = name;
            DefaultRowHeight = 22;
            DefaultColumnWidth = 70;
            WorkBook = book;
            Rows = new Rows(this);
            Columns = new Columns(this);
            TopLeft = new TopLeft(this);
            RowHeaders = new RowHeaders(this);
            ColumnHeaders = new ColumnHeaders(this);
            Cells = new Cells(this);
            RowCount = ColumnCount = 500;
            DataStore = new WorkSheetDataStore(this);
            FilterProvider = new FilterProvider(this);
        }

        public void SortRange(CellRange range, bool ascending)
        {
            Cells[range.TopRow, range.LeftColumn, range.RowCount, range.ColumnCount].Sort(ascending);
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
                DataStore.Dispose();
                DataStore = null;
                return;
            }

            if(DataStore != null)
            {
                DataStore.Dispose();
                DataStore = null;
            }

            DataStore = new WorkSheetDataStore(this, dataSource);          
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
            DataStore.Dispose();
            DataStore = null;
            DataSource = null;
            Rows.Dispose();
            Columns.Dispose();
            Cells.Dispose();
            RowHeaders.Dispose();
            ColumnHeaders.Dispose();
            Rows = null;
            Columns = null;
            Cells = null;
            RowHeaders = null;
            ColumnHeaders = null;
            TopLeft = null;
            FilterProvider = null;
            WorkBook = null;
        }

        public void Clear(WorkSheetClearMode mode)
        {
            switch(mode)
            {
                case WorkSheetClearMode.Data:
                    Cells.ClearCellStore();
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
    }
}
