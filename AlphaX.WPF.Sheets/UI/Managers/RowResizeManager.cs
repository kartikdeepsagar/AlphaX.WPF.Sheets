using System.Windows;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal class RowResizeManager : ResizeManagerBase
    {
        private int _rowLocation;
        private int _resizingRow;

        public bool IsResizing => _rowLocation != -1 && _resizingRow != -1;

        public RowResizeManager(AlphaXSpread spread) : base(spread)
        {
            _rowLocation = -1;
            _resizingRow = -1;
        }

        public void BeginResizeRow(int row, int rowLocation)
        {
            Spread.WorkBook.UpdateProvider.SuspendUpdates = true;
            ResizeLine.Visibility = Visibility.Visible;
            _rowLocation = rowLocation;
            _resizingRow = row;
        }

        public void ResizeRow(int currentLocation)
        {
            var newHeight = currentLocation - _rowLocation;

            if (newHeight < 0)
            {
                newHeight = 0;
                ResizeLine.Y1 = ResizeLine.Y2 = _rowLocation;
            }
            else
            {
                ResizeLine.Y1 = ResizeLine.Y2 = currentLocation;
            }

            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;

            ResizeLine.X1 = workSheet.RowHeaders.Width;
            ResizeLine.X2 = sheetView.Spread.SheetViewPane.ActualWidth;

            workSheet.Rows[_resizingRow].Height = newHeight;
            sheetView.ViewPort.As<ViewPort>().CalculateVisibleRange();
            sheetView.Invalidate(true, false, false, false);
        }

        public void EndResizeRow()
        {
            _resizingRow = -1;
            _rowLocation = -1;
            ResizeLine.Visibility = Visibility.Collapsed;
            Spread.SheetTabControl.UpdateScrollbars();
            Spread.WorkBook.UpdateProvider.SuspendUpdates = false;
        }
    }
}
