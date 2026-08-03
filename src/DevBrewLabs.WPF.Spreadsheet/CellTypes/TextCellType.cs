using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.Rendering;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class TextCellType : BaseCellType
    {
        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            base.DrawCell(context, value, style, formatter, cellRect, renderContext);

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                var align = style.HorizontalAlignment;
                if (value is string)
                {
                    if (align == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                        align = DevBrewLabs.Spreadsheet.HorizontalAlignment.Left;           
                }
                else
                {
                    if (align == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                        align = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

                    value = formatter.Format(value);
                }

                TextRenderer.DrawText(context, (string)value, cellRect, style, renderContext, align);
            }
        }

        public override EditorBase GetEditor(WPFStyle style)
        {
            var editor = new TextEditor();
            editor.FontFamily = style.WpfFontFamily;
            editor.Foreground = style.Foreground;
            editor.Background = style.Background;
            editor.FontWeight = style.WpfFontWeight;
            editor.FontStyle = style.WpfFontStyle;
            editor.FontSize = style.FontSize;
            return editor;
        }
    }
}
