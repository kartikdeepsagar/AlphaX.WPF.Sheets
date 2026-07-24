using System;

namespace AlphaX.Sheets
{
    public class SheetChangedEventArgs : EventArgs
    {
        protected SheetChangedEventArgs(SheetRegion region)
        {
            Region = region;
        }

        public SheetChangedEventArgs(IWorkSheet workSheet)
        {
            WorkSheet = workSheet;
        }

        public IWorkSheet WorkSheet { get; internal set; }
        internal SheetRegion Region { get; set; }
    }

    public sealed class CellChangedEventArgs : SheetChangedEventArgs
    {
        public CellChangedEventArgs(
            SheetRegion region,
            int row,
            int column,
            object oldValue,
            object newValue,
            CellChangeType changeType)
            : base(region)
        {
            Row = row;
            Column = column;
            OldValue = oldValue;
            NewValue = newValue;
            ChangeType = changeType;
        }

        public int Row { get; }

        public int Column { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public CellChangeType ChangeType { get; }
    }

    public sealed class RangeChangedEventArgs : SheetChangedEventArgs
    {
        public RangeChangedEventArgs(
            SheetRegion region,
            CellRange range,
            RangeChangeType changeType)
            : base(region)
        {
            Range = range;
            ChangeType = changeType;
        }

        public CellRange Range { get; }

        public RangeChangeType ChangeType { get; }
    }

    public sealed class RowChangedEventArgs : SheetChangedEventArgs
    {
        public RowChangedEventArgs(
            SheetRegion region,
            int index,
            int count,
            RowChangeType changeType)
            : base(region)
        {
            Index = index;
            Count = count;
            ChangeType = changeType;
        }

        public int Index { get; }

        public int Count { get; }

        public RowChangeType ChangeType { get; }
    }

    public sealed class ColumnChangedEventArgs : SheetChangedEventArgs
    {
        public ColumnChangedEventArgs(
            SheetRegion region,
            int index,
            int count,
            ColumnChangeType changeType)
            : base(region)
        {
            Index = index;
            Count = count;
            ChangeType = changeType;
        }

        public int Index { get; }

        public int Count { get; }

        public ColumnChangeType ChangeType { get; }
    }
}
