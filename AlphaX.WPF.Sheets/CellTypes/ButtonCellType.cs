using AlphaX.Sheets;
using AlphaX.Sheets.Formatters;
using AlphaX.WPF.Sheets.Rendering;
using AlphaX.WPF.Sheets.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.CellTypes
{
    public class ButtonCellType : BaseCellType
    {
        public IAlphaXCommand Command { get; set; }
        public string Text { get; set; }

        internal override void DrawCell(DrawingContext context, object value, Style style, IFormatter formatter, Rect cellRect, double pixelPerDip)
        {
            base.DrawCell(context, value, style, formatter, cellRect, pixelPerDip);

            cellRect.Inflate(-3, -3);
            context.DrawRectangle(Brushes.LightGray, null, cellRect);

            if(!string.IsNullOrEmpty(Text))
            {
                if (style.HorizontalAlignment == AlphaXHorizontalAlignment.Auto)
                    style.HorizontalAlignment = AlphaXHorizontalAlignment.Center;

                context.DrawText(Text, cellRect, style, pixelPerDip);
            }
        }

        public override AlphaXEditorBase GetEditor(Style style)
        {
            throw new NotImplementedException();
        }
    }
}
