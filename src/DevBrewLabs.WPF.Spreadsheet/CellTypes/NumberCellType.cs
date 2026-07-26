using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class NumberCellType : TextCellType
    {
        public string Format { get; set; }

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, double pixelPerDip, bool allowMultiLineText = true)
        {
            if (value == null)
                return;

            if(style.HorizontalAlignment == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                style.HorizontalAlignment = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

            if (!string.IsNullOrEmpty(Format))
                base.DrawCell(context, string.Format($"{{0:{Format}}}", value), style, formatter, cellRect, pixelPerDip, allowMultiLineText);
            else
                base.DrawCell(context, formatter.Format(value), style, formatter, cellRect, pixelPerDip, allowMultiLineText);
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