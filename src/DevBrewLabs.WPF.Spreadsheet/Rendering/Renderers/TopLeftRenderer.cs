using DevBrewLabs.Spreadsheet;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal class TopLeftRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            double zoom = SheetView.ZoomFactor > 0 ? SheetView.ZoomFactor : 1.0;
            var width = workSheet.RowHeaders.Width * zoom;
            var height = workSheet.ColumnHeaders.Height * zoom;
            var topLeft = workSheet.TopLeft;
            var style = workSheet.WorkBook.GetNamedStyle(string.IsNullOrEmpty(topLeft.StyleName) ? 
                StyleKeys.DefaultTopLeftStyleKey : topLeft.StyleName);
            var wpfStyle = style;

            double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
            var rect = new Rect(-SheetView.Spread.GridLinePen.Thickness, -SheetView.Spread.GridLinePen.Thickness, width, height);

            context.DrawRectangle(Styling.WpfResourceCache.GetBrush(wpfStyle.BackColor), SheetView.Spread.GridLinePen, rect);
       
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(new PathFigure(new Point(5 * zoom, height - 5 * zoom), new PathSegment[]
            {
                new LineSegment(new Point(width - 5 * zoom, 5 * zoom), false),
                new LineSegment(new Point(width - 5 * zoom, height - 5 * zoom), false),
                new LineSegment(new Point(5 * zoom, height - 5 * zoom), false)
            }, true));

            context.DrawGeometry(Styling.WpfResourceCache.GetBrush(wpfStyle.ForeColor), null, pathGeometry);
        }
    }
}



