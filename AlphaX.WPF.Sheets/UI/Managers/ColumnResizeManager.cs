using System;
using System.Windows;
using AlphaX.Sheets;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal class ColumnResizeManager : ResizeManagerBase
    {
        private int _columnLocation;
        private int _resizingColumn;
        private int[] _initialWidths;

        public bool IsResizing => _columnLocation != -1 && _resizingColumn != -1;

        public ColumnResizeManager(AlphaXSpread spread) : base(spread)
        {
            _columnLocation = -1;
            _resizingColumn = -1;           
        }

        public void BeginResizeColumn(int column, int columnLocation)
        {
            _columnLocation = columnLocation;
            _resizingColumn = column;

            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;

            _initialWidths = new int[workSheet.ColumnCount];
            for (int i = 0; i < workSheet.ColumnCount; i++)
            {
                _initialWidths[i] = workSheet.Columns.GetColumnWidth(i);
            }

            ResizeLine.Visibility = Visibility.Visible;
            Spread.SuspendUpdates = true;
        }

        public void ResizeColumn(int currentLocation)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;

            ResizeLine.X1 = ResizeLine.X2 = Math.Max(0, currentLocation);
            ResizeLine.Y1 = workSheet.ColumnHeaders.Height;
            ResizeLine.Y2 = sheetView.Spread.SheetViewPane.ActualHeight;

            if (_initialWidths == null || _resizingColumn < 0 || _resizingColumn >= workSheet.ColumnCount)
                return;

            if (currentLocation >= _columnLocation)
            {
                for (int c = 0; c < _resizingColumn; c++)
                {
                    workSheet.Columns[c].Width = _initialWidths[c];
                }

                var newWidth = currentLocation - _columnLocation;
                workSheet.Columns[_resizingColumn].Width = newWidth;

                for (int c = _resizingColumn + 1; c < workSheet.ColumnCount; c++)
                {
                    workSheet.Columns[c].Width = _initialWidths[c];
                }
            }
            else
            {
                workSheet.Columns[_resizingColumn].Width = 0;

                for (int c = _resizingColumn + 1; c < workSheet.ColumnCount; c++)
                {
                    workSheet.Columns[c].Width = _initialWidths[c];
                }

                double currentLeft = _columnLocation;
                int activeCol = -1;
                double activeColLeft = 0;

                for (int c = _resizingColumn - 1; c >= 0; c--)
                {
                    double colLeft = currentLeft - _initialWidths[c];
                    if (currentLocation >= colLeft)
                    {
                        activeCol = c;
                        activeColLeft = colLeft;
                        break;
                    }
                    currentLeft = colLeft;
                }

                if (activeCol != -1)
                {
                    for (int c = activeCol + 1; c < _resizingColumn; c++)
                    {
                        workSheet.Columns[c].Width = 0;
                    }

                    workSheet.Columns[activeCol].Width = Math.Max(0, (int)(currentLocation - activeColLeft));

                    for (int c = 0; c < activeCol; c++)
                    {
                        workSheet.Columns[c].Width = _initialWidths[c];
                    }
                }
                else
                {
                    for (int c = 0; c <= _resizingColumn; c++)
                    {
                        workSheet.Columns[c].Width = 0;
                    }
                }
            }

            sheetView.ViewPort.As<ViewPort>().CalculateVisibleRange();
            Spread.SheetViews.ActiveSheetView.Invalidate(false, true, false, false);
        }

        public void EndResizeColumn()
        {
            _resizingColumn = -1;
            _columnLocation = -1;
            _initialWidths = null;
            ResizeLine.Visibility = Visibility.Collapsed;
            Spread.SheetTabControl.UpdateScrollbars();
            Spread.SuspendUpdates = false;
        }
    }
}
