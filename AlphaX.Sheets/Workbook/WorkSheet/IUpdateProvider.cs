namespace AlphaX.Sheets
{
    internal interface IUpdateProvider
    {
        bool SuspendUpdates { get; set; }

        void CellChanged(
            WorkSheet worksheet,
            int row,
            int column,
            object oldValue,
            object newValue,
            SheetRegion region,
            CellChangeType changeType);

        void RangeChanged(
            WorkSheet worksheet,
            CellRange range,
            SheetRegion region,
            RangeChangeType changeType);

        void RowsChanged(
            WorkSheet worksheet,
            int index,
            int count,
            SheetRegion region,
            RowChangeType changeType);

        void ColumnsChanged(
            WorkSheet worksheet,
            int index,
            int count,
            SheetRegion region,
            ColumnChangeType changeType);
    }
}
