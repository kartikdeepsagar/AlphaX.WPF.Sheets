using DevBrewLabs.Spreadsheet.CalcEngine;

namespace DevBrewLabs.WPF.Spreadsheet
{
    public class CalcErrorEventArgs
    {
        public CalcEngineException Exception { get; internal set; }
        public ISheetView SheetView { get; internal set; }
        public int Row { get; internal set; }
        public int Column { get; internal set; }
        public string Formula { get; internal set; }
    }
}
