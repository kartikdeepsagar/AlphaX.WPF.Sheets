using System;
using DevBrewLabs.Spreadsheet;
using DevBrewLabs.WPF.Spreadsheet.UI;
using System.Windows;
using System.Windows.Media;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal class RowHeadersRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var rows = (Rows)workSheet.Rows;
            var columns = (RowHeaderColumns)workSheet.RowHeaders.Columns;
            var cells = (RowHeaderCells)workSheet.RowHeaders.Cells;
            var viewport = (ViewPort)SheetView.ViewPort;
            var workBook = (WorkBook)workSheet.WorkBook;
            
            double zoom = SheetView.ZoomFactor > 0 ? SheetView.ZoomFactor : 1.0;
            AdjustHeaderWidth(workSheet, rows, columns, cells, topRow, leftColumn, bottomRow, rightColumn);

            var renderContext = new RenderContext(zoom, SheetView.Spread.PixelPerDip, 5.0, true);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);

                if (rowHeight == 0)
                    continue;

                var sheetRow = rows.GetItem(row);
                var rowLocation = rows.GetLocation(row);
                var y = (rowLocation - viewport.TopRowLocation) * zoom;
                var scaledRowHeight = rowHeight * zoom;

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    var columnWidth = columns.GetColumnWidth(col);

                    if (columnWidth == 0)
                        continue;

                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col);
                    var colLocation = columns.GetLocation(col);
                    var x = colLocation * zoom;
                    var scaledColumnWidth = columnWidth * zoom;

                    var cellRect = new Rect(x, y, scaledColumnWidth, scaledRowHeight);
                    var baseStyle = workBook.PickStyle(cell, sheetColumn, sheetRow, SheetRegion.RowHeader);
                    var style = baseStyle.GetWpfStyle();

                    DrawRowHeaderCell(context, row, cell, style, cellRect, renderContext);
                }
            }
        }

        private void AdjustHeaderWidth(WorkSheet workSheet, Rows rows, RowHeaderColumns columns, RowHeaderCells cells, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            for (int col = leftColumn; col <= rightColumn; col++)
            {
                int headerWidth = (int)workSheet.RowHeaders.Columns[col].Width;
                int defaultColumnWidth = (int)workSheet.RowHeaders.DefaultColumnWidth;

                for (int row = topRow; row <= bottomRow; row++)
                {
                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col);
                    var sheetRow = rows.GetItem(row);
                    var style = ((WorkBook)workSheet.WorkBook).PickStyle(cell, sheetColumn, sheetRow, SheetRegion.RowHeader).GetWpfStyle();
                    var textWidth = TextMeasurer
                        .MeasureWidth(cell != null && cell.Value != null ? cell.Value.ToString() : (row + 1).ToString(), style.FontSize, style.GlyphMetrics);
                    textWidth += 10;

                    if (textWidth > headerWidth || (textWidth < headerWidth && textWidth > defaultColumnWidth))
                        headerWidth = (int)System.Math.Ceiling(textWidth);
                }

                if (headerWidth != workSheet.RowHeaders.Columns[col].Width)
                {
                    workSheet.RowHeaders.Columns[col].Width = headerWidth;
                    SheetView.Spread.SheetViewPane.UpdateHeadersSize();
                }
            }
        }

        private void DrawRowHeaderCell(DrawingContext context, int row, IRange cell, WPFStyle style, Rect cellRect, RenderContext renderContext)
        {
            context.DrawRectangle(style.Background, null, cellRect);

            if (cell != null && cell.Value != null)
            {
                TextRenderer.DrawText(context, cell.Value.ToString(), cellRect, style, renderContext);
            }
            else
            {
                TextRenderer.DrawText(context, (row + 1).ToString(), cellRect, style, renderContext);
            }
        }
    }
}
