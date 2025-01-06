using System.Windows.Controls;

namespace AlphaX.WPF.Sheets.UI.Editors
{
    public class AlphaXEditorBase : TextBox, IEditorInfo
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public IAlphaXSheetView SheetView { get; set; }

        public virtual void SetValue(object value)
        {
            Text = value?.ToString();
        }
    }
}
