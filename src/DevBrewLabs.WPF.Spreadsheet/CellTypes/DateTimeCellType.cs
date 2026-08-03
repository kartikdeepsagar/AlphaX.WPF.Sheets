using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public class DateTimeCellType : TextCellType
    {
        public string Format { get; set; } = "d";

        internal override void DrawCell(DrawingContext context, object value, WPFStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            if (value == null)
                return;

            if (style.HorizontalAlignment == DevBrewLabs.Spreadsheet.HorizontalAlignment.Auto)
                style.HorizontalAlignment = DevBrewLabs.Spreadsheet.HorizontalAlignment.Right;

            DateTime? date = null;
            if (value is DateTime)
                date = (DateTime)value;
            else if (value is string s && DateTime.TryParse(s, out var parsed))
                date = parsed;
            else if (value is double d)
                date = DateTime.FromOADate(d);

            if (date.HasValue)
            {
                base.DrawCell(context, date.Value.ToString(Format), style, formatter, cellRect, renderContext);
            }
            else
            {
                base.DrawCell(context, value.ToString(), style, formatter, cellRect, renderContext);
            }
        }

        public override EditorBase GetEditor(WPFStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
