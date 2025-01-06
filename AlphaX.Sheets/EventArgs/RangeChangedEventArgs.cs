using System;

namespace AlphaX.Sheets
{
    public class RangeChangedEventArgs : BaseEventArgs
    {
        public CellRange CellRange { get; internal set; }
        public SortState SortState { get; internal set; }
    }
}
