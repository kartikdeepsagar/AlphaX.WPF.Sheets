using AlphaX.Sheets;
using AlphaX.WPF.Sheets.UI;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class CellsRegion : AlphaXSheetViewRegion
    {
        private WorkSheet _workSheet;
        private DrawingGroup _drawing;
        private ViewPort _viewPort;
        private double _dragFillOffset;

        public CellsRegion()
        {
            _drawing = new DrawingGroup();
            _dragFillOffset = 5;
        }

        public override void AttachSheet(AlphaXSheetView sheetView)
        {
            base.AttachSheet(sheetView);
            _workSheet = sheetView.WorkSheet;
            _drawing.Children.Clear();
            _drawing.Children.Add(sheetView.Spread.RenderEngine.GridLinesRenderer.Drawing);
            _drawing.Children.Add(sheetView.Spread.RenderEngine.CellsRenderer.Drawing);

            _viewPort = sheetView.ViewPort.As<ViewPort>();
        }

        protected override Drawing GetDrawing()
        {
            return _drawing;
        }

        protected override SpreadHitTestResult HitTestCore(AlphaXSheetView sheetView, Point hitPoint)
        {
            var hitTestInfo = new SpreadHitTestResult() { Element = VisualElement.Cell, Row = -1, Column = -1, Sheet = sheetView };
            hitTestInfo.ActualHitTestPoint = hitPoint;
            var rows = _workSheet.Rows.As<Rows>();
            var columns = _workSheet.Columns.As<Columns>();
            var viewRange = sheetView.ViewPort.ViewRange;

            var point = new Point(hitPoint.X + _viewPort.LeftColumnLocation, 
                hitPoint.Y + _viewPort.TopRowLocation);

            double x = 0, y = 0;

            for (int row = viewRange.TopRow; row <= viewRange.BottomRow; row++)
            {
                var rowLocation = rows.GetLocation(row);
                double rowHeight = _workSheet.Rows.GetRowHeight(row);

                if (point.Y >= rowLocation && point.Y < rowLocation + rowHeight)
                {
                    hitTestInfo.Row = row;
                    y = rowLocation;
                    for (int col = viewRange.LeftColumn; col <= viewRange.RightColumn; col++)
                    {
                        var colLocation = columns.GetLocation(col);
                        double columnWidth = _workSheet.Columns.GetColumnWidth(col);

                        if (point.X >= colLocation && point.X < colLocation + columnWidth)
                        {
                            hitTestInfo.Column = col;
                            x = colLocation;

                            if (sheetView.Selection.RightColumn == hitTestInfo.Column && sheetView.Selection.BottomRow == hitTestInfo.Row)
                            {
                                if (point.X > x + columnWidth - _dragFillOffset && point.Y > y + rowHeight - _dragFillOffset
                                    && point.X <= x + columnWidth && point.Y <= y + rowHeight)
                                    hitTestInfo.Element = VisualElement.DragFill;
                            }

                            break;
                        }
                    }
                    break;
                }
            }
            
            hitTestInfo.Position = new Point(x - _viewPort.LeftColumnLocation, 
                y - _viewPort.TopRowLocation);
            return hitTestInfo;
        }
    }
}