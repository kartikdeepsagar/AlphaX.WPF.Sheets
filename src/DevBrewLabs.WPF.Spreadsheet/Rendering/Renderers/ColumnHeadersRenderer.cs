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
            double halfPenWidth = SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip / 2;
            
            var renderContext = new RenderContext(zoom, SheetView.Spread.PixelPerDip, 5.0, true);
            
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);
                if (rowHeight == 0)
                    continue;
                var sheetRow = rows.GetItem(row);
                var rowLocation = rows.GetLocation(row);
                var y = rowLocation * zoom;
                var scaledRowHeight = rowHeight * zoom;

                guidelines.GuidelinesY.Add(y + halfPenWidth);
                guidelines.GuidelinesY.Add(y + scaledRowHeight + halfPenWidth);

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

                    if (row == topRow)
                    {
                        guidelines.GuidelinesX.Add(x + halfPenWidth);
                        guidelines.GuidelinesX.Add(x + scaledColumnWidth + halfPenWidth);
                    }

                    var cellRect = new Rect(x, y, scaledColumnWidth, scaledRowHeight);

                    var baseStyle = workBook.PickStyle(cell, sheetColumn, sheetRow, SheetRegion.ColumnHeader);
                    var style = baseStyle.GetWpfStyle();

                    DrawColumnHeaderCell(context, row, col, cell, style, cellRect, renderContext);
                }

                // Render double vertical lines for hidden columns
                int minCol = Math.Max(0, leftColumn);
                int maxCol = Math.Min(workSheet.ColumnCount - 1, rightColumn + 1);

                for (int col = minCol; col <= maxCol; col++)
                {
                    if (columns.GetColumnWidth(col) == 0)
                    {
                        // Draw double line indicator only for the first hidden column in a contiguous block
                        if (col == 0 || columns.GetColumnWidth(col - 1) > 0)
                        {
                            var colLocation = columns.GetLocation(col);
                            var x = (colLocation - viewport.LeftColumnLocation) * zoom;
                            DrawHiddenColumnIndicator(context, x, y, scaledRowHeight, workSheet);
                        }
                    }
                }
            }

            context.Pop();
        }

        private void DrawHiddenColumnIndicator(DrawingContext context, double x, double rowLocation, double rowHeight, WorkSheet workSheet)
        {
            var pen = SheetView.Spread.GridLinePen;
            var defaultStyle = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultColumnHeaderStyleKey).GetWpfStyle();

            double line1X, line2X;
            if (x <= 0)
            {
                line1X = x + 1.5;
                line2X = x + 4.5;
            }
            else
            {
                line1X = x - 1.5;
                line2X = x + 1.5;
            }

            var rectLeft = Math.Min(line1X, line2X) - 0.5;
            var rectWidth = Math.Abs(line2X - line1X) + 1.0;
            var gapRect = new Rect(rectLeft, rowLocation, rectWidth, rowHeight);

            if (defaultStyle != null && defaultStyle.Background != null)
            {
                context.DrawRectangle(defaultStyle.Background, null, gapRect);
            }

            context.DrawLine(pen, new Point(line1X, rowLocation), new Point(line1X, rowLocation + rowHeight));
            context.DrawLine(pen, new Point(line2X, rowLocation), new Point(line2X, rowLocation + rowHeight));
        }

        private void DrawColumnHeaderCell(DrawingContext context, int row, int column, IRange cell, WPFStyle style, Rect cellRect, DevBrewLabs.WPF.Spreadsheet.Rendering.Text.RenderContext renderContext)
        {
            context.DrawRectangle(style.Background, SheetView.Spread.GridLinePen, cellRect);
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
