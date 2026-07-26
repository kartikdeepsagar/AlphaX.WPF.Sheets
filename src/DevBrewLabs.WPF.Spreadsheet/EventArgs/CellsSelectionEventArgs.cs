using DevBrewLabs.Spreadsheet;
using System;

namespace DevBrewLabs.WPF.Spreadsheet
{
    public class CellsSelectionEventArgs : EventArgs
    {
        public ISheetView SheetView { get; internal set; }
        public CellRange Selection { get; internal set; }
    }
}
