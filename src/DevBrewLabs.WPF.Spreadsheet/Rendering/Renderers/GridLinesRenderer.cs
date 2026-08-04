
using DevBrewLabs.Spreadsheet;
using DevBrewLabs.WPF.Spreadsheet.UI;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
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
            double zoom = SheetView.ZoomFactor > 0 ? SheetView.ZoomFactor : 1.0;

            double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);
                if (rowHeight == 0)
                    continue;

                var rowLocation = rows.GetLocation(row);
                double y = (rowLocation - viewport.TopRowLocation + rowHeight) * zoom;
                guidelines.GuidelinesY.Add(y + halfPenWidth);
                double rightX = (columns.GetLocation(rightColumn) - viewport.LeftColumnLocation + columns.GetColumnWidth(rightColumn)) * zoom;
                context.DrawLine(SheetView.Spread.GridLinePen, new Point(0, y),
                            new Point(rightX, y));
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
            double zoom = SheetView.ZoomFactor > 0 ? SheetView.ZoomFactor : 1.0;

            double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int col = leftColumn; col <= rightColumn; col++)
            {
                var columnWidth = columns.GetColumnWidth(col);
                if (columnWidth == 0)
                    continue;

                var colLocation = columns.GetLocation(col);
                double x = (colLocation - viewport.LeftColumnLocation + columnWidth) * zoom;
                guidelines.GuidelinesX.Add(x + halfPenWidth);
                double bottomY = (rows.GetLocation(bottomRow) - viewport.TopRowLocation + rows.GetRowHeight(bottomRow)) * zoom;
                context.DrawLine(SheetView.Spread.GridLinePen, new Point(x, 0),
                    new Point(x, bottomY));
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
