using System.Windows.Controls;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.UI.Editors
{
    public class EditorBase : TextBox, IEditorInfo
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public ISheetView SheetView { get; set; }

        public EditorBase()
        {
            TextOptions.SetTextFormattingMode(this, System.Windows.Media.TextFormattingMode.Ideal);
            TextOptions.SetTextRenderingMode(this, System.Windows.Media.TextRenderingMode.Auto);
        }

        public virtual void SetValue(object value)
        {
            Text = value?.ToString();
        }
    }
}
