using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class DateTimeCellType : TextCellType
    {
        public string Format { get; set; }

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, double pixelPerDip, bool allowMultiLineText = true)
        {
            if (value == null)
                return;

            if (style.HorizontalAlignment == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                style.HorizontalAlignment = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

            if (!string.IsNullOrEmpty(Format))
                base.DrawCell(context, ((DateTime)value).ToString(Format), style, formatter, cellRect, pixelPerDip, allowMultiLineText);
            else
                base.DrawCell(context, formatter.Format(value), style, formatter, cellRect, pixelPerDip, allowMultiLineText);
        }

        public override EditorBase GetEditor(WPFStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
