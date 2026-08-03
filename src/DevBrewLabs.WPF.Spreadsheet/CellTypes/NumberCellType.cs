using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class NumberCellType : TextCellType
    {
        public string Format { get; set; }

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            if (value == null)
                return;

            if (style.HorizontalAlignment == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                style.HorizontalAlignment = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

            if (!string.IsNullOrEmpty(Format))
                base.DrawCell(context, string.Format($"{{0:{Format}}}", value), style, formatter, cellRect, renderContext);
            else
                base.DrawCell(context, formatter.Format(value), style, formatter, cellRect, renderContext);
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