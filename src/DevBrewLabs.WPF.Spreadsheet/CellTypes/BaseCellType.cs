using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Editors;
using System.Windows;
using System.Windows.Media;

using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;

namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public abstract class BaseCellType : ICellType
    {
        internal virtual void DrawCell(DrawingContext drawingContext, object value, WPFStyle style, IFormatter formatter, Rect cellRect, RenderContext renderContext)
        {
            if (style.BackColor != DevBrewLabs.Spreadsheet.Drawing.Color.Transparent)
            {
                drawingContext.DrawRectangle(style.Background, null, cellRect);
            }
        }

        /// <summary>
        /// Gets the editor for cell type
        /// </summary>
        /// <returns></returns>
        public abstract EditorBase GetEditor(WPFStyle style);
    }
}
