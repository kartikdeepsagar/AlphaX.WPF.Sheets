using AlphaX.Sheets;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal class SelectionManager : UIManager, ISelectionManager
    {
        public SelectionManager(AlphaXSpread spread) : base(spread)
        {
           
        }

        public void SelectCell(int row, int col)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            sheetView.ActiveRow = row;
            sheetView.ActiveColumn = col;
            SelectRange(row, col, 1, 1);
        }

        public void SelectColumn(int column)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;
            sheetView.ActiveRow = 0;
            sheetView.ActiveColumn = column;
            SelectRange(0, column, workSheet.RowCount, 1);
        }

        public void SelectColumns(int column, int count)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;
            SelectRange(0, column, workSheet.RowCount, count);
        }

        public void SelectRow(int row)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;
            sheetView.ActiveRow = row;
            sheetView.ActiveColumn = 0;
            SelectRange(row, 0, 1, workSheet.ColumnCount);
        }

        public void SelectRows(int row, int count)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var workSheet = sheetView.WorkSheet;
            SelectRange(row, 0, count, workSheet.ColumnCount);
        }

        public void SelectRange(CellRange range)
        {
            SelectRange(range.TopRow, range.LeftColumn, range.RowCount, range.ColumnCount);
        }

        public void SelectRange(int row, int column, int rowCount, int columnCount)
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            var selection = sheetView.Selection;
            var workSheet = sheetView.WorkSheet;

            if (!workSheet.ContainsRange(row, column, rowCount, columnCount))
                return;

            switch (sheetView.SelectionMode)
            {
                case SelectionMode.Column:
                case SelectionMode.Columns:
                    selection.TopRow = 0;
                    selection.LeftColumn = column;
                    selection.RowCount = workSheet.RowCount;
                    selection.ColumnCount = columnCount;
                    break;

                case SelectionMode.Row:
                case SelectionMode.Rows:
                    selection.TopRow = row;
                    selection.LeftColumn = 0;
                    selection.RowCount = rowCount;
                    selection.ColumnCount = workSheet.ColumnCount;
                    break;

                case SelectionMode.CellRange:
                case SelectionMode.Cell:
                    selection.TopRow = row;
                    selection.LeftColumn = column;
                    selection.RowCount = rowCount;
                    selection.ColumnCount = columnCount;
                    break;
            }

            sheetView.Spread.RaiseCellsSelectionChanged(new CellsSelectionEventArgs()
            {
                SheetView = sheetView,
                Selection = sheetView.Selection
            });
            RefreshInteractionLayers();
        }

        private void RefreshInteractionLayers()
        {
            var cellsInteractionLayer = Spread.SheetViewPane.CellsRegion.GetInteractionLayer();

            if (cellsInteractionLayer != null && cellsInteractionLayer.IsLoaded)
                cellsInteractionLayer.InvalidateVisual();

            var rowHeadersInteractionLayer = Spread.SheetViewPane.RowHeadersRegion.GetInteractionLayer();

            if (rowHeadersInteractionLayer != null && rowHeadersInteractionLayer.IsLoaded)
                rowHeadersInteractionLayer.InvalidateVisual();

            var columnHeadersInteractionLayer = Spread.SheetViewPane.ColumnHeadersRegion.GetInteractionLayer();

            if (columnHeadersInteractionLayer != null && columnHeadersInteractionLayer.IsLoaded)
                columnHeadersInteractionLayer.InvalidateVisual();
        }
    }
}
