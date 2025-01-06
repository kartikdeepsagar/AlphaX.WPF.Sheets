using System;

namespace AlphaX.Sheets
{
    public class CellChangedEventArgs : BaseEventArgs
    {
        public int Row { get; internal set; }
        public int Column { get; internal set; }
    }
}
