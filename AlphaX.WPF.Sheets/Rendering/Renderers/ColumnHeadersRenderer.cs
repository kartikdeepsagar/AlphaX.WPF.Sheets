using AlphaX.Sheets;

using AlphaX.WPF.Sheets.UI;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class ColumnHeadersRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var rows = workSheet.ColumnHeaders.Rows;
            var columns = workSheet.Columns;
            var cells = workSheet.ColumnHeaders.Cells;
            var viewport = SheetView.ViewPort.As<ViewPort>();

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

                guidelines.GuidelinesY.Add(rowLocation + halfPenWidth);
                guidelines.GuidelinesY.Add(rowLocation + rowHeight + halfPenWidth);

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
                        var x = colLocation - viewport.LeftColumnLocation;
                        guidelines.GuidelinesX.Add(x + halfPenWidth);
                        guidelines.GuidelinesX.Add(x + columnWidth + halfPenWidth);
                    }

                    var cellRect = new Rect(colLocation - viewport.LeftColumnLocation, rowLocation, columnWidth, rowHeight);

                    var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow);

                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultColumnHeaderStyleKey);

                    DrawColumnHeaderCell(context, row, col, cell, style, cellRect, SheetView.Spread.PixelPerDip);
                }
            }

            context.Pop();
        }

        private void DrawColumnHeaderCell(DrawingContext context, int row, int column, ICell cell, IStyle baseStyle, Rect cellRect, double pixelPerDip)
        {
            var style = baseStyle.As<Style>();

            context.DrawRectangle(style.Background, SheetView.Spread.GridLinePen, cellRect);
            if (cell != null && cell.Value != null)
            {
                context.DrawText(cell.Value.ToString(), cellRect, style, pixelPerDip, true);
            }
            else
            {
                context.DrawText(RenderingExtensions.GetColumnHeader(column), cellRect, style, pixelPerDip);
            }
        }      
    }
}
