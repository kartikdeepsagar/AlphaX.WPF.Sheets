namespace AlphaX.WPF.Sheets.UI.Editors
{
    public interface IEditorInfo
    {
        int Row { get; set; }
        int Column { get; set; }
        IAlphaXSheetView SheetView { get; set; }
        void SetValue(object value);
    }
}
