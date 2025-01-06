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

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);
                if (rowHeight == 0)
                    continue;
                var sheetRow = rows.GetItem(row, false);
                var rowLocation = rows.GetLocation(row);

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    if (Engine.RenderInfo.PartialRender)
                        Engine.EnsureNewCacheDrawing(this, row, col);
                    var columnWidth = columns.GetColumnWidth(col);
                    if (columnWidth == 0)
                        continue;

                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col, false);
                    var colLocation = columns.GetLocation(col);

                    var cellRect = new Rect(colLocation - viewport.LeftColumnLocation, rowLocation, columnWidth, rowHeight);

                    var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow);

                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultColumnHeaderStyleKey);

                    style = style.Clone();
                    var cellDrawing = Engine.CreateDrawingObject(this, row, col);

                    double halfPenWidth = SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip / 2;
                    GuidelineSet guidelines = new GuidelineSet();
                    guidelines.GuidelinesX.Add(cellRect.Left + halfPenWidth);
                    guidelines.GuidelinesX.Add(cellRect.Right + halfPenWidth);
                    guidelines.GuidelinesY.Add(cellRect.Top + halfPenWidth);
                    guidelines.GuidelinesY.Add(cellRect.Bottom + halfPenWidth);
                    var ctx = cellDrawing.Open();
                    ctx.PushClip(new RectangleGeometry(cellRect));
                    ctx.PushGuidelineSet(guidelines);
                    DrawColumnHeaderCell(ctx, row, col, cell, style, cellRect, SheetView.Spread.PixelPerDip);
                    ctx.Pop();
                    ctx.Pop();
                    ctx.Close();
                    context.DrawDrawing(cellDrawing);
                    style.Dispose();
                    style = null;
                }
            }
        }

        private void DrawColumnHeaderCell(DrawingContext context, int row, int column, ICell cell, IStyle baseStyle, Rect cellRect, double pixelPerDip)
        {
            var style = baseStyle.As<Style>();
            double halfPenWidth = SheetView.Spread.GridLinePen.Thickness * pixelPerDip / 2;
            GuidelineSet guidelines = new GuidelineSet();
            guidelines.GuidelinesX.Add(cellRect.Left + halfPenWidth);
            guidelines.GuidelinesX.Add(cellRect.Right + halfPenWidth);
            guidelines.GuidelinesY.Add(cellRect.Top + halfPenWidth);
            guidelines.GuidelinesY.Add(cellRect.Bottom + halfPenWidth);

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
