using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.Rendering;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class ButtonCellType : BaseCellType
    {
        public ICellTypeCommand Command { get; set; }
        public string Text { get; set; }

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, double pixelPerDip, bool allowMultiLineText = true, double zoomFactor = 1.0)
        {
            base.DrawCell(context, value, style, formatter, cellRect, pixelPerDip, allowMultiLineText, zoomFactor);

            cellRect.Inflate(-3 * zoomFactor, -3 * zoomFactor);
            context.DrawRectangle(Brushes.LightGray, null, cellRect);

            if(!string.IsNullOrEmpty(Text))
            {
                if (style.HorizontalAlignment == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                    style.HorizontalAlignment = DevBrewLabs.Spreadsheet.HorizontalAlignment.Center;

                var renderContext = new DevBrewLabs.WPF.Spreadsheet.Rendering.Text.RenderContext(zoomFactor, pixelPerDip, 5.0, true);
                DevBrewLabs.WPF.Spreadsheet.Rendering.Text.TextRenderer.DrawText(context, Text, cellRect, style, renderContext, false, allowMultiLineText);
            }
        }

        public override EditorBase GetEditor(WPFStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
