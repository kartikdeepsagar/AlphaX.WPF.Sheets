using AlphaX.CalcEngine;

namespace AlphaX.WPF.Sheets
{
    public class CalcErrorEventArgs
    {
        public CalcEngineException Exception { get; internal set; }
        public IAlphaXSheetView SheetView { get; internal set; }
        public int Row { get; internal set; }
        public int Column { get; internal set; }
        public string Formula { get; internal set; }
    }
}
