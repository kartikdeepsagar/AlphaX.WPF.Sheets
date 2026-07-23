using AlphaX.Sheets;
using AlphaX.Sheets.Formatters;
using AlphaX.WPF.Sheets.Rendering;
using AlphaX.WPF.Sheets.UI.Editors;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.CellTypes
{
    public class TextCellType : BaseCellType
    {
        internal override void DrawCell(DrawingContext context, object value, AlphaXStyle style, IFormatter formatter, Rect cellRect, double pixelPerDip, bool allowMultiLineText = true)
        {
            base.DrawCell(context, value, style, formatter, cellRect, pixelPerDip, allowMultiLineText);

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                if (value is string)
                {
                    if (style.HorizontalAlignment == AlphaXHorizontalAlignment.Auto)
                        style.HorizontalAlignment = AlphaXHorizontalAlignment.Left;           
                }
                else
                {
                    if (style.HorizontalAlignment == AlphaXHorizontalAlignment.Auto)
                        style.HorizontalAlignment = AlphaXHorizontalAlignment.Right;

                    value = formatter.Format(value);
                }

                context.DrawText((string)value, cellRect, style, pixelPerDip, false, allowMultiLineText);
            }
        }

        public override AlphaXEditorBase GetEditor(AlphaXStyle style)
        {
            var editor = new AlphaXTextBox();
            editor.FontFamily = style.WpfFontFamily;
            editor.Foreground = style.Foreground;
            editor.Background = style.Background;
            editor.FontSize = style.FontSize;
            return editor;
        }
    }
}
