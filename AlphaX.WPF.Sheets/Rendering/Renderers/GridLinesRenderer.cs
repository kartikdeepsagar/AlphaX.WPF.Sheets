
using AlphaX.WPF.Sheets.UI;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class GridLinesRenderer : Renderer
    {
        /// <summary>
        /// Draws horizontal grid lines.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topRow"></param>
        /// <param name="bottomRow"></param>
        private void DrawHorizontalGridlines(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var rows = workSheet.Rows;
            var columns = workSheet.Columns;
            var viewport = SheetView.ViewPort.As<ViewPort>();

            for (int row = topRow; row <= bottomRow; row++)
            {
                if (Engine.RenderInfo.PartialRender)
                    Engine.EnsureNewCacheDrawing(this, row, -1);
                var rowHeight = rows.GetRowHeight(row);
                if (rowHeight == 0)
                    continue;

                var rowLocation = rows.GetLocation(row);
                double y = rowLocation - viewport.TopRowLocation + rowHeight;
                double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
                DrawingGroup drawing = Engine.CreateDrawingObject(this, row, -1);
                GuidelineSet guidelines = new GuidelineSet();
                guidelines.GuidelinesY.Add(y + halfPenWidth);
                drawing.GuidelineSet = guidelines;
                var ctx = drawing.Open();

                ctx.DrawLine(SheetView.Spread.GridLinePen, new Point(0, y),
                            new Point(columns.GetLocation(rightColumn) - viewport.LeftColumnLocation + columns.GetColumnWidth(rightColumn), y));
                ctx.Close();
                context.DrawDrawing(drawing);
            }
        }

        /// <summary>
        /// Draws vertical grid lines.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="leftColumn"></param>
        /// <param name="rightColumn"></param>
        private void DrawVerticalGridlines(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var rows = workSheet.Rows;
            var columns = workSheet.Columns;
            var viewport = SheetView.ViewPort.As<ViewPort>();

            for (int col = leftColumn; col <= rightColumn; col++)
            {
                if (Engine.RenderInfo.PartialRender)
                    Engine.EnsureNewCacheDrawing(this, -1, col);
                var columnWidth = columns.GetColumnWidth(col);
                if (columnWidth == 0)
                    continue;

                var colLocation = columns.GetLocation(col);
                double x = colLocation - viewport.LeftColumnLocation + columnWidth;
                double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
                DrawingGroup drawing = Engine.CreateDrawingObject(this, -1, col);
                GuidelineSet guidelines = new GuidelineSet();
                guidelines.GuidelinesX.Add(x + halfPenWidth);
                drawing.GuidelineSet = guidelines;
                var ctx = drawing.Open();

                ctx.DrawLine(SheetView.Spread.GridLinePen, new Point(x, 0),
                    new Point(x, rows.GetLocation(bottomRow) - viewport.TopRowLocation + rows.GetRowHeight(bottomRow)));
                ctx.Close();
                context.DrawDrawing(drawing);
            }
        }

        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            switch (SheetView.GridLineVisibility)
            {
                case GridLineVisibility.Vertical:
                    DrawVerticalGridlines(context, topRow, leftColumn, bottomRow, rightColumn);
                    break;

                case GridLineVisibility.Horizontal:
                    DrawHorizontalGridlines(context, topRow, leftColumn, bottomRow, rightColumn);
                    break;

                case GridLineVisibility.Both:
                    DrawVerticalGridlines(context, topRow, leftColumn, bottomRow, rightColumn);
                    DrawHorizontalGridlines(context, topRow, leftColumn, bottomRow, rightColumn);
                    break;
            }
        }
    }
}
