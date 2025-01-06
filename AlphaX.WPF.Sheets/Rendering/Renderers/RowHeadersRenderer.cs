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
            var rows = workSheet.Rows;
            var columns = workSheet.RowHeaders.Columns;
            var cells = workSheet.RowHeaders.Cells;
            var viewport = SheetView.ViewPort.As<ViewPort>();
            
            AdjustHeaderWidth(workSheet, rows, columns, cells, topRow, leftColumn, bottomRow, rightColumn);
            
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

                    var cellRect = new Rect(colLocation, rowLocation - viewport.TopRowLocation, columnWidth, rowHeight);
                    var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow);

                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey);

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
                    DrawRowHeaderCell(ctx, row, cell, style, cellRect, SheetView.Spread.PixelPerDip);
                    ctx.Pop();
                    ctx.Pop();
                    ctx.Close();
                    context.DrawDrawing(cellDrawing);
                    style.Dispose();
                    style = null;
                }
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
                    var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow).As<Style>();
                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey).As<Style>();
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

        private void DrawRowHeaderCell(DrawingContext context, int row, ICell cell, IStyle baseStyle, Rect cellRect, double pixelPerDip)
        {
            var style = baseStyle.As<Style>();
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
