namespace DevBrewLabs.WPF.Spreadsheet
{
    public class SheetViewEventArgs
    {
        public ISheetView OldSheetView { get; internal set; }
        public ISheetView NewSheetView { get; internal set; }
    }
}
