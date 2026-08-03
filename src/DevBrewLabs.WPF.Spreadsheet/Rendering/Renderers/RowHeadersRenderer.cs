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
                var y = (rowLocation - viewport.TopRowLocation) * zoom;
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
                    var x = colLocation * zoom;
                    var scaledColumnWidth = columnWidth * zoom;

                    if (row == topRow)
                    {
                        guidelines.GuidelinesX.Add(x + halfPenWidth);
                        guidelines.GuidelinesX.Add(x + scaledColumnWidth + halfPenWidth);
                    }

                    var cellRect = new Rect(x, y, scaledColumnWidth, scaledRowHeight);
                    var baseStyle = workBook.PickStyle(cell, sheetColumn, sheetRow, SheetRegion.RowHeader);
                    var style = baseStyle.GetWpfStyle();

                    DrawRowHeaderCell(context, row, cell, style, cellRect, renderContext);
                }
            }

            // Render double horizontal lines for hidden rows
            int minRow = Math.Max(0, topRow);
            int maxRow = Math.Min(workSheet.RowCount - 1, bottomRow + 1);

            for (int row = minRow; row <= maxRow; row++)
            {
                if (rows.GetRowHeight(row) == 0)
                {
                    if (row == 0 || rows.GetRowHeight(row - 1) > 0)
                    {
                        var rowLocation = rows.GetLocation(row);
                        var y = (rowLocation - viewport.TopRowLocation) * zoom;
                        DrawHiddenRowIndicator(context, y, leftColumn, rightColumn, columns, workSheet, zoom);
                    }
                }
            }

            context.Pop();
        }

        private void DrawHiddenRowIndicator(DrawingContext context, double y, int leftColumn, int rightColumn, RowHeaderColumns columns, WorkSheet workSheet, double zoom)
        {
            var pen = SheetView.Spread.GridLinePen;
            var defaultStyle = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey).GetWpfStyle();

            double line1Y, line2Y;
            if (y <= 0)
            {
                line1Y = y + 1.5;
                line2Y = y + 4.5;
            }
            else
            {
                line1Y = y - 1.5;
                line2Y = y + 1.5;
            }

            var rectTop = Math.Min(line1Y, line2Y) - 0.5;
            var rectHeight = Math.Abs(line2Y - line1Y) + 1.0;

            for (int col = leftColumn; col <= rightColumn; col++)
            {
                var columnWidth = columns.GetColumnWidth(col);
                if (columnWidth == 0)
                    continue;
                var colLocation = columns.GetLocation(col) * zoom;
                var scaledColumnWidth = columnWidth * zoom;
                var gapRect = new Rect(colLocation, rectTop, scaledColumnWidth, rectHeight);

                if (defaultStyle != null && defaultStyle.Background != null)
                {
                    context.DrawRectangle(defaultStyle.Background, null, gapRect);
                }

                context.DrawLine(pen, new Point(colLocation, line1Y), new Point(colLocation + scaledColumnWidth, line1Y));
                context.DrawLine(pen, new Point(colLocation, line2Y), new Point(colLocation + scaledColumnWidth, line2Y));
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
                    var textWidth = DevBrewLabs.WPF.Spreadsheet.Rendering.Text.TextMeasurer
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

        private void DrawRowHeaderCell(DrawingContext context, int row, IRange cell, WPFStyle style, Rect cellRect, DevBrewLabs.WPF.Spreadsheet.Rendering.Text.RenderContext renderContext)
        {
            context.DrawRectangle(style.Background, SheetView.Spread.GridLinePen, cellRect);

            if (cell != null && cell.Value != null)
            {
                DevBrewLabs.WPF.Spreadsheet.Rendering.Text.TextRenderer.DrawText(context, cell.Value.ToString(), cellRect, style, renderContext);
            }
            else
            {
                DevBrewLabs.WPF.Spreadsheet.Rendering.Text.TextRenderer.DrawText(context, (row + 1).ToString(), cellRect, style, renderContext);
            }
        }
    }
}
