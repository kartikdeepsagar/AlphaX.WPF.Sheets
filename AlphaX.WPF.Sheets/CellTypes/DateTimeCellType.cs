using AlphaX.Sheets;
using AlphaX.Sheets.Formatters;
using AlphaX.WPF.Sheets.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.CellTypes
{
    public class DateTimeCellType : TextCellType
    {
        public string Format { get; set; }

        internal override void DrawCell(DrawingContext context, object value, AlphaXStyle style, IFormatter formatter, Rect cellRect, double pixelPerDip, bool allowMultiLineText = true)
        {
            if (value == null)
                return;

            if (style.HorizontalAlignment == AlphaXHorizontalAlignment.Auto)
                style.HorizontalAlignment = AlphaXHorizontalAlignment.Right;

            if (!string.IsNullOrEmpty(Format))
                base.DrawCell(context, ((DateTime)value).ToString(Format), style, formatter, cellRect, pixelPerDip, allowMultiLineText);
            else
                base.DrawCell(context, formatter.Format(value), style, formatter, cellRect, pixelPerDip, allowMultiLineText);
        }

        public override AlphaXEditorBase GetEditor(AlphaXStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
