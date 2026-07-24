using System.Windows.Controls;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.UI.Editors
{
    public class AlphaXEditorBase : TextBox, IEditorInfo
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public IAlphaXSheetView SheetView { get; set; }

        public AlphaXEditorBase()
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
