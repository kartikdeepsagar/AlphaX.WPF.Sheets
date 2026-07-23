using AlphaX.Sheets;
using AlphaX.WPF.Sheets.UI;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class ColumnHeadersRegion : AlphaXSheetViewRegion
    {
        private WorkSheet _workSheet;
        private ViewPort _viewPort;
        private readonly int _resizeDelta;

        public ColumnHeadersRegion()
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
            return SheetView.Spread.RenderEngine.ColumnHeadersRenderer.Drawing;
        }

        protected override SpreadHitTestResult HitTestCore(AlphaXSheetView sheetView, Point hitPoint)
        {
            var hitTestInfo = new SpreadHitTestResult() { Element = VisualElement.ColumnHeader, Sheet = sheetView };
            hitTestInfo.ActualHitTestPoint = hitPoint;
            var rows = _workSheet.ColumnHeaders.Rows.As<Rows>();
            var columns = _workSheet.Columns.As<Columns>();
            var viewRange = sheetView.ViewPort.ViewRange;

            var point = new Point(hitPoint.X + _viewPort.LeftColumnLocation,
                hitPoint.Y + _viewPort.TopRowLocation);

            double x = 0, y = 0;

            for (int row = viewRange.TopRow; row <= viewRange.BottomRow; row++)
            {
                var rowLocation = rows.GetLocation(row);
                var sheetRow = rows.GetItem(row, false);
                double rowHeight = sheetRow == null ? _workSheet.DefaultRowHeight : sheetRow.Height;

                if (point.Y >= rowLocation && point.Y < rowLocation + rowHeight)
                {
                    hitTestInfo.Row = row;
                    y = rowLocation;
                    break;
                }
            }

            // Check for hidden column resize handle hit first (only if custom column settings exist)
            if (columns.InternalCollection.Count > 0)
            {
                int startColSearch = Math.Max(0, _viewPort.ViewRange.LeftColumn - 1);
                int endColSearch = Math.Min(_workSheet.ColumnCount, _viewPort.ViewRange.RightColumn + 2);

                for (int col = startColSearch; col < endColSearch; col++)
                {
                    if (columns.GetColumnWidth(col) == 0)
                    {
                        int startHiddenCol = col;
                        int lastHiddenCol = col;
                        while (lastHiddenCol + 1 < _workSheet.ColumnCount && columns.GetColumnWidth(lastHiddenCol + 1) == 0)
                        {
                            lastHiddenCol++;
                        }

                        var colLocation = columns.GetLocation(startHiddenCol);
                        bool isHit;
                        if (colLocation == 0)
                        {
                            isHit = point.X >= 0 && point.X <= _resizeDelta + 2;
                        }
                        else
                        {
                            isHit = point.X >= colLocation - 2 && point.X <= colLocation + _resizeDelta;
                        }

                        if (isHit)
                        {
                            hitTestInfo.Element = VisualElement.ColumnHeaderResizeBar;
                            hitTestInfo.Column = lastHiddenCol;
                            x = colLocation;
                            hitTestInfo.Position = new Point(x - _viewPort.LeftColumnLocation,
                                y - _viewPort.TopRowLocation);
                            return hitTestInfo;
                        }

                        col = lastHiddenCol;
                    }
                }
            }

            // Check visible column resize boundaries (centered around right edge)
            for (int col = viewRange.LeftColumn; col <= viewRange.RightColumn; col++)
            {
                var colLocation = columns.GetLocation(col);
                double columnWidth = _workSheet.Columns.GetColumnWidth(col);

                if (columnWidth == 0)
                    continue;

                double rightEdge = colLocation + columnWidth;
                if (point.X >= rightEdge - _resizeDelta && point.X <= rightEdge + _resizeDelta)
                {
                    hitTestInfo.Element = VisualElement.ColumnHeaderResizeBar;
                    hitTestInfo.Column = col;
                    x = colLocation;
                    hitTestInfo.Position = new Point(x - _viewPort.LeftColumnLocation,
                        y - _viewPort.TopRowLocation);
                    return hitTestInfo;
                }
            }

            // Check visible column body hit
            for (int col = viewRange.LeftColumn; col <= viewRange.RightColumn; col++)
            {
                var colLocation = columns.GetLocation(col);
                double columnWidth = _workSheet.Columns.GetColumnWidth(col);

                if (columnWidth == 0)
                    continue;

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
