using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;
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

        internal override void DrawCell(DrawingContext context, object value, IStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            base.DrawCell(context, value, style, formatter, cellRect, renderContext);

            cellRect.Inflate(-3 * renderContext.Zoom, -3 * renderContext.Zoom);
            context.DrawRectangle(Brushes.LightGray, null, cellRect);

            if(!string.IsNullOrEmpty(Text))
            {
                var align = style.HorizontalAlignment;
                if (align == CellHorizontalAlignment.Auto)
                    align = CellHorizontalAlignment.Center;

                TextRenderer.DrawText(context, Text, cellRect, style, renderContext, align);
            }
        }

        public override EditorBase GetEditor(IStyle style)
        {
            throw new NotImplementedException();
        }
    }
}

