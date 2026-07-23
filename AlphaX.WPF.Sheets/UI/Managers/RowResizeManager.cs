using System;
using System.Windows;
using AlphaX.Sheets;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal class RowResizeManager : ResizeManagerBase
    {
        private int _rowLocation;
        private int _resizingRow;
        private int[] _initialHeights;

        public bool IsResizing => _rowLocation != -1 && _resizingRow != -1;

        public RowResizeManager(AlphaXSpread spread) : base(spread)
        {
            _rowLocation = -1;
            _resizingRow = -1;
        }

        public void BeginResizeRow(int row, int rowLocation)
        {
            _rowLocation = rowLocation;
            _resizingRow = row;

            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;

            _initialHeights = new int[workSheet.RowCount];
            for (int i = 0; i < workSheet.RowCount; i++)
            {
                _initialHeights[i] = workSheet.Rows.GetRowHeight(i);
            }

            ResizeLine.Visibility = Visibility.Visible;
            Spread.WorkBook.UpdateProvider.SuspendUpdates = true;
        }

        public void ResizeRow(int currentLocation)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;

            ResizeLine.Y1 = ResizeLine.Y2 = Math.Max(0, currentLocation);
            ResizeLine.X1 = workSheet.RowHeaders.Width;
            ResizeLine.X2 = sheetView.Spread.SheetViewPane.ActualWidth;

            if (_initialHeights == null || _resizingRow < 0 || _resizingRow >= workSheet.RowCount)
                return;

            if (currentLocation >= _rowLocation)
            {
                for (int r = 0; r < _resizingRow; r++)
                {
                    workSheet.Rows[r].Height = _initialHeights[r];
                }

                var newHeight = currentLocation - _rowLocation;
                workSheet.Rows[_resizingRow].Height = newHeight;

                for (int r = _resizingRow + 1; r < workSheet.RowCount; r++)
                {
                    workSheet.Rows[r].Height = _initialHeights[r];
                }
            }
            else
            {
                workSheet.Rows[_resizingRow].Height = 0;

                for (int r = _resizingRow + 1; r < workSheet.RowCount; r++)
                {
                    workSheet.Rows[r].Height = _initialHeights[r];
                }

                double currentTop = _rowLocation;
                int activeRow = -1;
                double activeRowTop = 0;

                for (int r = _resizingRow - 1; r >= 0; r--)
                {
                    double rowTop = currentTop - _initialHeights[r];
                    if (currentLocation >= rowTop)
                    {
                        activeRow = r;
                        activeRowTop = rowTop;
                        break;
                    }
                    currentTop = rowTop;
                }

                if (activeRow != -1)
                {
                    for (int r = activeRow + 1; r < _resizingRow; r++)
                    {
                        workSheet.Rows[r].Height = 0;
                    }

                    workSheet.Rows[activeRow].Height = Math.Max(0, (int)(currentLocation - activeRowTop));

                    for (int r = 0; r < activeRow; r++)
                    {
                        workSheet.Rows[r].Height = _initialHeights[r];
                    }
                }
                else
                {
                    for (int r = 0; r <= _resizingRow; r++)
                    {
                        workSheet.Rows[r].Height = 0;
                    }
                }
            }

            sheetView.ViewPort.As<ViewPort>().CalculateVisibleRange();
            sheetView.Invalidate(true, false, false, false);
        }

        public void EndResizeRow()
        {
            _resizingRow = -1;
            _rowLocation = -1;
            _initialHeights = null;
            ResizeLine.Visibility = Visibility.Collapsed;
            Spread.SheetTabControl.UpdateScrollbars();
            Spread.WorkBook.UpdateProvider.SuspendUpdates = false;
        }
    }
}
