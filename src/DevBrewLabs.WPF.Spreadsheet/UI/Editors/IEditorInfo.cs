namespace DevBrewLabs.WPF.Spreadsheet.UI.Editors
{
    public interface IEditorInfo
    {
        int Row { get; set; }
        int Column { get; set; }
        ISheetView SheetView { get; set; }
        void SetValue(object value);
    }
}
