using AlphaX.Sheets;
using AlphaX.Sheets.Formatters;
using AlphaX.WPF.Sheets.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.CellTypes
{
    public class NumberCellType : TextCellType
    {
        public string Format { get; set; }

        internal override void DrawCell(DrawingContext context, object value, Style style, IFormatter formatter, Rect cellRect, double pixelPerDip)
        {
            if (value == null)
                return;

            if(style.HorizontalAlignment == AlphaXHorizontalAlignment.Auto)
                style.HorizontalAlignment = AlphaXHorizontalAlignment.Right;

            if (!string.IsNullOrEmpty(Format))
                base.DrawCell(context, string.Format($"{{0:{Format}}}", value), style, formatter, cellRect, pixelPerDip);
            else
                base.DrawCell(context, formatter.Format(value), style, formatter, cellRect, pixelPerDip);
        }

        /// <inheritdoc/>
        public override AlphaXEditorBase GetEditor(Style style)
        {
            var editor = new AlphaXNumericEditor() { TextAlignment = TextAlignment.Right };
            editor.FontFamily = style.WpfFontFamily;
            editor.Foreground = style.Foreground;
            editor.Background = style.Background;
            editor.FontSize = style.FontSize;
            return editor;
        }
    }
}