using System.Windows;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal class ColumnResizeManager : ResizeManagerBase
    {
        private int _columnLocation;
        private int _resizingColumn;

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
            ResizeLine.Visibility = Visibility.Visible;
            Spread.WorkBook.UpdateProvider.SuspendUpdates = true;
        }

        public void ResizeColumn(int currentLocation)
        {
            var newWidth = currentLocation - _columnLocation;

            if (newWidth < 0)
            {
                newWidth = 0;
                ResizeLine.X1 = ResizeLine.X2 = _columnLocation;
            }
            else
            {
                ResizeLine.X1 = ResizeLine.X2 = currentLocation;
            }

            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;

            ResizeLine.Y1 = workSheet.ColumnHeaders.Height;
            ResizeLine.Y2 = sheetView.Spread.SheetViewPane.ActualHeight;

            workSheet.Columns[_resizingColumn].Width = newWidth;
            sheetView.ViewPort.As<ViewPort>().CalculateVisibleRange();
            Spread.SheetViews.ActiveSheetView.Invalidate(false, true, false, false);
        }

        public void EndResizeColumn()
        {
            _resizingColumn = -1;
            _columnLocation = -1;
             ResizeLine.Visibility = Visibility.Collapsed;
            Spread.SheetTabControl.UpdateScrollbars();
            Spread.WorkBook.UpdateProvider.SuspendUpdates = false;
        }
    }
}
