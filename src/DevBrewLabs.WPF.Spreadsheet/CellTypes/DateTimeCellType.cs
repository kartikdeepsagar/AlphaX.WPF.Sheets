using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class DateTimeCellType : BaseCellType
    {
        public string Format { get; set; } = "d";

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            base.DrawCell(context, value, style, formatter, cellRect, renderContext);

            if (value == null)
                return;

            var align = style.HorizontalAlignment;
            if (align == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                align = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

            DateTime? date = null;
            if (value is DateTime)
                date = (DateTime)value;
            else if (value is string s && DateTime.TryParse(s, out var parsed))
                date = parsed;
            else if (value is double d)
                date = DateTime.FromOADate(d);

            string textToDraw;
            if (date.HasValue)
            {
                textToDraw = date.Value.ToString(Format);
            }
            else
            {
                textToDraw = value.ToString();
            }
            
            TextRenderer.DrawText(context, textToDraw, cellRect, style, renderContext, align);
        }

        public override EditorBase GetEditor(WPFStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
