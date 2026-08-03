using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class NumberCellType : BaseCellType
    {
        public string Format { get; set; }

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            base.DrawCell(context, value, style, formatter, cellRect, renderContext);

            if (value == null)
                return;

            var align = style.HorizontalAlignment;
            if (align == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                align = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

            string textToDraw;
            if (!string.IsNullOrEmpty(Format))
                textToDraw = string.Format($"{{0:{Format}}}", value);
            else
                textToDraw = formatter.Format(value);

            TextRenderer.DrawText(context, textToDraw, cellRect, style, renderContext, align);
        }

        /// <inheritdoc/>
        public override EditorBase GetEditor(WPFStyle style)
        {
            var editor = new NumericEditor() { TextAlignment = TextAlignment.Right };
            editor.FontFamily = style.WpfFontFamily;
            editor.Foreground = style.Foreground;
            editor.Background = style.Background;
            editor.FontSize = style.FontSize;
            return editor;
        }
    }
}