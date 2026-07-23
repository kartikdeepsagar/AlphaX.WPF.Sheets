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
}
