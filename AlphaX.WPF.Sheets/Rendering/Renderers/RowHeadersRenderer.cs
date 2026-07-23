using System;
using AlphaX.Sheets;
using AlphaX.WPF.Sheets.UI;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class RowHeadersRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var rows = (Rows)workSheet.Rows;
            var columns = (Columns)workSheet.RowHeaders.Columns;
            var cells = (Cells)workSheet.RowHeaders.Cells;
            var viewport = (ViewPort)SheetView.ViewPort;
            var workBook = (WorkBook)workSheet.WorkBook;
            
            AdjustHeaderWidth(workSheet, rows, columns, cells, topRow, leftColumn, bottomRow, rightColumn);

            double halfPenWidth = SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip / 2;
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);

                if (rowHeight == 0)
                    continue;

                var sheetRow = rows.GetItem(row, false);
                var rowLocation = rows.GetLocation(row);
                var y = rowLocation - viewport.TopRowLocation;

                guidelines.GuidelinesY.Add(y + halfPenWidth);
                guidelines.GuidelinesY.Add(y + rowHeight + halfPenWidth);

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    var columnWidth = columns.GetColumnWidth(col);

                    if (columnWidth == 0)
                        continue;

                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col, false);
                    var colLocation = columns.GetLocation(col);

                    if (row == topRow)
                    {
                        guidelines.GuidelinesX.Add(colLocation + halfPenWidth);
                        guidelines.GuidelinesX.Add(colLocation + columnWidth + halfPenWidth);
                    }

                    var cellRect = new Rect(colLocation, y, columnWidth, rowHeight);
                    var style = workBook.PickStyle(cell, sheetColumn, sheetRow);

                    if (style == null)
                        style = workBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey);

                    DrawRowHeaderCell(context, row, cell, style, cellRect, SheetView.Spread.PixelPerDip);
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
                        var y = rowLocation - viewport.TopRowLocation;
                        DrawHiddenRowIndicator(context, y, leftColumn, rightColumn, columns, workSheet);
                    }
                }
            }

            context.Pop();
        }

        private void DrawHiddenRowIndicator(DrawingContext context, double y, int leftColumn, int rightColumn, Columns columns, WorkSheet workSheet)
        {
            var pen = SheetView.Spread.GridLinePen;
            var defaultStyle = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey).As<AlphaXStyle>();

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
                var colLocation = columns.GetLocation(col);
                var gapRect = new Rect(colLocation, rectTop, columnWidth, rectHeight);

                if (defaultStyle != null && defaultStyle.Background != null)
                {
                    context.DrawRectangle(defaultStyle.Background, null, gapRect);
                }

                context.DrawLine(pen, new Point(colLocation, line1Y), new Point(colLocation + columnWidth, line1Y));
                context.DrawLine(pen, new Point(colLocation, line2Y), new Point(colLocation + columnWidth, line2Y));
            }
        }

        private void AdjustHeaderWidth(WorkSheet workSheet, Rows rows, Columns columns, Cells cells, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            for (int col = leftColumn; col <= rightColumn; col++)
            {
                var headerWidth = workSheet.RowHeaders.Columns[col].Width;
                var defaultColumnWidth = workSheet.RowHeaders.DefaultColumnWidth;

                for (int row = topRow; row <= bottomRow; row++)
                {
                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col, false);
                    var sheetRow = rows.GetItem(row, false);
                    var style = ((WorkBook)workSheet.WorkBook).PickStyle(cell, sheetColumn, sheetRow).As<AlphaXStyle>();
                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey).As<AlphaXStyle>();
                    var textWidth = TextRenderingExtensions
                        .ComputeTextWidth(cell != null && cell.Value != null ? cell.Value.ToString() : (row + 1).ToString(), style.FontSize, style.GlyphTypeface);
                    textWidth += 10;

                    if (textWidth > headerWidth || (textWidth < headerWidth && textWidth > defaultColumnWidth))
                        headerWidth = textWidth;
                }

                if (headerWidth != workSheet.RowHeaders.Columns[col].Width)
                {
                    workSheet.RowHeaders.Columns[col].Width = headerWidth;
                    SheetView.Spread.SheetViewPane.UpdateHeadersSize();
                }
            }
        }

        private void DrawRowHeaderCell(DrawingContext context, int row, IRange cell, IStyle baseStyle, Rect cellRect, double pixelPerDip)
        {
            var style = baseStyle.As<AlphaXStyle>();
            context.DrawRectangle(style.Background, SheetView.Spread.GridLinePen, cellRect);

            if (cell != null && cell.Value != null)
            {
                context.DrawText(cell.Value.ToString(), cellRect, style, pixelPerDip);
            }
            else
            {
                context.DrawText((row + 1).ToString(), cellRect, style, pixelPerDip);
            }
        }
    }
}
