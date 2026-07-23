using AlphaX.Sheets;
using AlphaX.WPF.Sheets.UI;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class RowHeadersRegion : AlphaXSheetViewRegion
    {
        private WorkSheet _workSheet;
        private ViewPort _viewPort;
        private readonly int _resizeDelta;

        public RowHeadersRegion()
        {
            _resizeDelta = 5;
        }

        public override void AttachSheet(AlphaXSheetView sheetView)
        {
            base.AttachSheet(sheetView);
            _workSheet = sheetView.WorkSheet;
            _viewPort = sheetView.ViewPort.As<ViewPort>();
        }

        protected override Drawing GetDrawing()
        {
            return SheetView.Spread.RenderEngine.RowHeadersRenderer.Drawing;
        }

        protected override SpreadHitTestResult HitTestCore(AlphaXSheetView sheetView, Point hitPoint)
        {
            var hitTestInfo = new SpreadHitTestResult() { Element = VisualElement.RowHeader, Sheet = sheetView };
            hitTestInfo.ActualHitTestPoint = hitPoint;
            var rows = _workSheet.Rows.As<Rows>();
            var columns = _workSheet.RowHeaders.Columns.As<Columns>();
            var viewRange = sheetView.ViewPort.ViewRange;

            var point = new Point(hitPoint.X + _viewPort.LeftColumnLocation,
                hitPoint.Y + _viewPort.TopRowLocation);

            double x = 0, y = 0;

            // Check for hidden row resize handle hit first
            for (int row = 0; row < _workSheet.RowCount; row++)
            {
                if (rows.GetRowHeight(row) == 0)
                {
                    int startHiddenRow = row;
                    int lastHiddenRow = row;
                    while (lastHiddenRow + 1 < _workSheet.RowCount && rows.GetRowHeight(lastHiddenRow + 1) == 0)
                    {
                        lastHiddenRow++;
                    }

                    var rowLocation = rows.GetLocation(startHiddenRow);
                    bool isHit;
                    if (rowLocation == 0)
                    {
                        isHit = point.Y >= 0 && point.Y <= _resizeDelta + 2;
                    }
                    else
                    {
                        isHit = point.Y >= rowLocation - 2 && point.Y <= rowLocation + _resizeDelta;
                    }

                    if (isHit)
                    {
                        hitTestInfo.Element = VisualElement.RowHeaderResizeBar;
                        hitTestInfo.Row = lastHiddenRow;
                        y = rowLocation;
                        hitTestInfo.Position = new Point(x - _viewPort.LeftColumnLocation,
                            y - _viewPort.TopRowLocation);
                        return hitTestInfo;
                    }

                    row = lastHiddenRow;
                }
            }

            // Check visible row resize boundaries (centered around bottom edge)
            for (int row = viewRange.TopRow; row <= viewRange.BottomRow; row++)
            {
                var rowLocation = rows.GetLocation(row);
                double rowHeight = _workSheet.Rows.GetRowHeight(row);

                if (rowHeight == 0)
                    continue;

                double bottomEdge = rowLocation + rowHeight;
                if (point.Y >= bottomEdge - _resizeDelta && point.Y <= bottomEdge + _resizeDelta)
                {
                    hitTestInfo.Element = VisualElement.RowHeaderResizeBar;
                    hitTestInfo.Row = row;
                    y = rowLocation;
                    hitTestInfo.Position = new Point(x - _viewPort.LeftColumnLocation,
                        y - _viewPort.TopRowLocation);
                    return hitTestInfo;
                }
            }

            // Check visible row body hit
            for (int row = viewRange.TopRow; row <= viewRange.BottomRow; row++)
            {
                var rowLocation = rows.GetLocation(row);
                double rowHeight = _workSheet.Rows.GetRowHeight(row);

                if (rowHeight == 0)
                    continue;

                if (point.Y >= rowLocation && point.Y < rowLocation + rowHeight)
                {
                    hitTestInfo.Row = row;
                    y = rowLocation;
                    break;
                }
            }

            for (int col = viewRange.LeftColumn; col <= viewRange.RightColumn; col++)
            {
                var colLocation = columns.GetLocation(col);
                var sheetColumn = columns.GetItem(col, false);
                double columnWidth = sheetColumn == null ? _workSheet.DefaultColumnWidth : sheetColumn.Width;

                if (point.X >= colLocation && point.X < colLocation + columnWidth)
                {
                    hitTestInfo.Column = col;
                    x = colLocation;
                    break;
                }
            }

            hitTestInfo.Position = new Point(x - _viewPort.LeftColumnLocation,
                y - _viewPort.TopRowLocation);
            return hitTestInfo;
        }
    }
}
