using AlphaX.Sheets;
using System;

namespace AlphaX.WPF.Sheets
{
    public class CellsSelectionEventArgs : EventArgs
    {
        public IAlphaXSheetView SheetView { get; internal set; }
        public CellRange Selection { get; internal set; }
    }
}
