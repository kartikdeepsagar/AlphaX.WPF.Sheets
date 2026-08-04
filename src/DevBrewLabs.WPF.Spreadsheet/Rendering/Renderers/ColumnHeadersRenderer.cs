using System;
using System.Windows;
using System.Windows.Media;
using DevBrewLabs.Spreadsheet;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;
using DevBrewLabs.WPF.Spreadsheet.UI;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal class ColumnHeadersRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var workBook = (WorkBook)workSheet.WorkBook;
            var rows = (ColumnHeaderRows)workSheet.ColumnHeaders.Rows;
            var columns = (Columns)workSheet.Columns;
            var cells = (ColumnHeaderCells)workSheet.ColumnHeaders.Cells;
            var viewport = (ViewPort)SheetView.ViewPort;

            double zoom = SheetView.ZoomFactor > 0 ? SheetView.ZoomFactor : 1.0;
            var renderContext = new RenderContext(zoom, SheetView.Spread.PixelPerDip, 5.0, true);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);
                if (rowHeight == 0)
                    continue;
                var sheetRow = rows.GetItem(row);
                var rowLocation = rows.GetLocation(row);
                var y = rowLocation * zoom;
                var scaledRowHeight = rowHeight * zoom;

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    var columnWidth = columns.GetColumnWidth(col);
                    if (columnWidth == 0)
                        continue;

                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col);
                    var colLocation = columns.GetLocation(col);
                    var x = (colLocation - viewport.LeftColumnLocation) * zoom;
                    var scaledColumnWidth = columnWidth * zoom;

                    var cellRect = new Rect(x, y, scaledColumnWidth, scaledRowHeight);

                    var baseStyle = workBook.PickStyle(cell, sheetColumn, sheetRow, SheetRegion.ColumnHeader);
                    var style = baseStyle.GetWpfStyle();

                    DrawColumnHeaderCell(context, row, col, cell, style, cellRect, renderContext);
                }
            }
        }

        private void DrawColumnHeaderCell(DrawingContext context, int row, int column, IRange cell, WPFStyle style, Rect cellRect, DevBrewLabs.WPF.Spreadsheet.Rendering.Text.RenderContext renderContext)
        {
            context.DrawRectangle(style.Background, null, cellRect);
            if (cell != null && cell.Value != null)
            {
                DevBrewLabs.WPF.Spreadsheet.Rendering.Text.TextRenderer.DrawText(context, cell.Value.ToString(), cellRect, style, renderContext);
            }
            else
            {
                DevBrewLabs.WPF.Spreadsheet.Rendering.Text.TextRenderer.DrawText(context, RenderingExtensions.GetColumnHeader(column), cellRect, style, renderContext);
            }
        }      
    }
}
