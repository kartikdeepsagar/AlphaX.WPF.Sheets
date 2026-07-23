
using AlphaX.Sheets;
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
            var rows = (Rows)workSheet.Rows;
            var columns = (Columns)workSheet.Columns;
            var viewport = (ViewPort)SheetView.ViewPort;

            double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);
                if (rowHeight == 0)
                    continue;

                var rowLocation = rows.GetLocation(row);
                double y = rowLocation - viewport.TopRowLocation + rowHeight;
                guidelines.GuidelinesY.Add(y + halfPenWidth);
                context.DrawLine(SheetView.Spread.GridLinePen, new Point(0, y),
                            new Point(columns.GetLocation(rightColumn) - viewport.LeftColumnLocation + columns.GetColumnWidth(rightColumn), y));
            }

            context.Pop();
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
            var rows = (Rows)workSheet.Rows;
            var columns = (Columns)workSheet.Columns;
            var viewport = (ViewPort)SheetView.ViewPort;

            double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int col = leftColumn; col <= rightColumn; col++)
            {
                var columnWidth = columns.GetColumnWidth(col);
                if (columnWidth == 0)
                    continue;

                var colLocation = columns.GetLocation(col);
                double x = colLocation - viewport.LeftColumnLocation + columnWidth;
                guidelines.GuidelinesX.Add(x + halfPenWidth);
                context.DrawLine(SheetView.Spread.GridLinePen, new Point(x, 0),
                    new Point(x, rows.GetLocation(bottomRow) - viewport.TopRowLocation + rows.GetRowHeight(bottomRow)));
            }

            context.Pop();
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
