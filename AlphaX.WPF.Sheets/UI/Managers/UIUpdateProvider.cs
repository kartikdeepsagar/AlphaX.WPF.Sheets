using AlphaX.Sheets;
using System;
using System.Windows.Threading;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal sealed class UIUpdateProvider : IUpdateProvider
    {
        private AlphaXSpread _spread;
        private bool _suspendUpdates;

        public UIUpdateProvider(AlphaXSpread spread)
        {
            _spread = spread;
        }

        bool IUpdateProvider.SuspendUpdates
        {
            get
            {
                return _suspendUpdates;
            }
            set
            {
                _suspendUpdates = value;

                if(!_suspendUpdates && _spread.IsLoaded)
                {
                    _spread.SheetViews.ActiveSheetView.Invalidate();
                }
            }
        }

        void IUpdateProvider.CellChanged(WorkSheet worksheet, int row, int column, object oldValue, object newValue, SheetRegion region, CellChangeType changeType)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {

                if (!_spread.IsLoaded)
                    return;

                var sheetView = _spread.SheetViews.GetSheetView(worksheet);

                if (!sheetView.ViewPort.ViewRange.ContainsCell(row, column))
                    return;
      
                if (sheetView.ViewPort.ViewRange.ContainsCell(row, column))
                {
                    switch (changeType)
                    {
                        case CellChangeType.Value:
                        case CellChangeType.Formula:
                            worksheet.AutoSizeRow(row);
                            sheetView.Invalidate();
                            break;

                        case CellChangeType.Style:
                            sheetView.InvalidateCellRange(row, column, row, column);
                            break;
                    }
                }
            }));
        }

        void IUpdateProvider.ColumnsChanged(WorkSheet worksheet, int index, int count, SheetRegion region, ColumnChangeType changeType)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                if (!_spread.IsLoaded)
                    return;

                var sheetView = _spread.SheetViews.GetSheetView(worksheet);
                sheetView.ViewPort.As<ViewPort>().CalculateVisibleRange();
                if (sheetView.ViewPort.ViewRange.ContainsColumn(index))
                {
                    _spread.SheetTabControl.UpdateScrollbars();
                    sheetView.Invalidate(false, true, true, false);
                }
            }));
        }

        void IUpdateProvider.RangeChanged(WorkSheet worksheet, CellRange range, SheetRegion region, RangeChangeType changeType)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                if (!_spread.IsLoaded)
                    return;

                var sheetView = _spread.SheetViews.GetSheetView(worksheet);
                if (range == null || !range.IsValid || sheetView.ViewPort.ViewRange.Intersects(range))
                {
                    sheetView.Invalidate(true, false, true, false);
                }
            }));
        }

        void IUpdateProvider.RowsChanged(WorkSheet worksheet, int index, int count, SheetRegion region, RowChangeType changeType)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                if (!_spread.IsLoaded)
                    return;

                var sheetView = _spread.SheetViews.GetSheetView(worksheet);
                sheetView.ViewPort.As<ViewPort>().CalculateVisibleRange();
                if (sheetView.ViewPort.ViewRange.ContainsRow(index))
                {
                    _spread.SheetTabControl.UpdateScrollbars();
                    sheetView.Invalidate(true, false, true, false);
                }
            }));
        }
    }
}
