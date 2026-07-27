using DevBrewLabs.Spreadsheet;
using DevBrewLabs.WPF.Spreadsheet.Rendering;
using DevBrewLabs.WPF.Spreadsheet.UI;
using System;
using System.Text;
using System.Windows;
using System.Collections.Generic;

namespace DevBrewLabs.WPF.Spreadsheet
{
    internal class SheetView : ISheetView, IDisposable
    {
        private HeadersVisibility _headersVisibility;
        private ViewPort _viewPort;
        private WorkSheet _workSheet;
        private WorkBook _workBook;
        private Rows _rows;
        private Cells _cells;
        private Columns _columns;
        private double _zoomFactor = 1.0;

        public event EventHandler<ZoomChangedEventArgs> ZoomChanged;

        #region Properties
        public GridLineVisibility GridLineVisibility { get; set; }
        public HeadersVisibility HeadersVisibility
        {
            get
            {
                return _headersVisibility;
            }
            set
            {
                _headersVisibility = value;
                SetHeadersVisibility();
            }
        }
        public double ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                var clamped = Math.Max(0.1, Math.Min(4.0, Math.Round(value, 2)));
                if (Math.Abs(_zoomFactor - clamped) > 0.001)
                {
                    var oldVal = _zoomFactor;
                    _zoomFactor = clamped;
                    ZoomChanged?.Invoke(this, new ZoomChangedEventArgs(oldVal, _zoomFactor));
                    if (Spread.SheetViews?.ActiveSheetView == this)
                    {
                        Spread.SheetViewPane?.UpdateZoomTransform();
                        _viewPort?.CalculateVisibleRange();
                        Spread.SheetTabControl?.UpdateScrollbars();
                        Spread.Invalidate();
                    }
                }
            }
        }
        public IViewPort ViewPort => _viewPort;
        public Point ScrollPosition { get; private set; }
        public SelectionMode SelectionMode { get; set; }
        public MouseWheelScrollDirection MouseWheelScrollDirection { get; set; }
        public Spread Spread { get; }
        public int ActiveRow { get; internal set; }
        public int ActiveColumn { get; internal set; }
        public CellRange Selection { get; }
        public WorkSheet WorkSheet => _workSheet;
        #endregion

        public SheetView(Spread spread, WorkSheet worksheet)
        {
            Spread = spread;
            _workSheet = worksheet;
            _workBook = (WorkBook)_workSheet.WorkBook;
            _rows = (Rows)_workSheet.Rows;
            _columns = (Columns)_workSheet.Columns;
            _cells = (Cells)_workSheet.Cells;
            _zoomFactor = 1.0;
            GridLineVisibility = GridLineVisibility.Both;
            SelectionMode = SelectionMode.CellRange;
            MouseWheelScrollDirection = MouseWheelScrollDirection.Vertical;
            ScrollPosition = new Point(0, 0);
            _viewPort = new ViewPort(this);
            HeadersVisibility = HeadersVisibility.Both;
            Selection = new CellRange(0, 0);
        }

        #region Public
        public void CopyToClipboard()
        {
            Spread.ClipboardManager.Copy(this);
        }

        public void PasteFromClipboard()
        {
            Spread.ClipboardManager.Paste();
        }

        public void CopyToClipboard(CellRange range)
        {
            Spread.ClipboardManager.Copy(this, range);
        }

        public void ScrollToHorizontalOffset(double offset)
        {
            double delta = offset - ScrollPosition.X;
            ScrollPosition = new Point(offset, ScrollPosition.Y);
            _viewPort.CalculateLeftColumn(delta);
            _viewPort.CalculateVisibleRange();
            Spread.Invalidate(false, true, true);
        }

        public void ScrollToVerticalOffset(double offset)
        {
            double delta = offset - ScrollPosition.Y;
            ScrollPosition = new Point(ScrollPosition.X, offset);
            _viewPort.CalculateTopRow(delta);
            _viewPort.CalculateVisibleRange();
            Spread.Invalidate(true, false, true);
        }
        #endregion

        #region Private
        private void SetHeadersVisibility()
        {
            Spread.SheetViewPane.UpdateHeadersSize();
        }
        #endregion

        #region Internal
        internal double GetRowHeaderWidth()
        {
            if (HeadersVisibility == HeadersVisibility.Row || HeadersVisibility == HeadersVisibility.Both)
                return _workSheet.RowHeaders.Width;
            else return 0;
        }

        internal double GetColumnHeaderHeight()
        {
            if (HeadersVisibility == HeadersVisibility.Column || HeadersVisibility == HeadersVisibility.Both)
                return _workSheet.ColumnHeaders.Height;
            else return 0;
        }

        #endregion

        public override string ToString()
        {
            return _workSheet.Name;
        }

        public void AutoSizeColumn(int column)
        {
            var sheetColumn = _columns.GetItem(column);
            var width = 0;
            var cellValues = _cells.GetCellValues(column);

            foreach(var cellValue in cellValues)
            {
                if(cellValue.Value != null)
                {
                    var style = _workBook.PickStyle(_cells.GetCell(cellValue.Key, column, false), sheetColumn, _rows.GetItem(cellValue.Key), SheetRegion.RowHeader);
                    var textWidth = TextRenderingExtensions.ComputeTextWidth(cellValue.Value.ToString(), style.FontSize, style.GetWpfStyle()?.GlyphTypeface);
                    width = Math.Max(width, textWidth + 11);
                }
            }

            if (width == 0)
            {
                width = WorkSheet.DefaultColumnWidth;
            }

            if(width != WorkSheet.Columns.GetColumnWidth(column))
            {
                WorkSheet.Columns[column].Width = width;
            }

            _viewPort.CalculateVisibleRange();
            Spread.SheetTabControl.UpdateScrollbars();
            Spread.Invalidate();
        }

        public void Dispose()
        {
            _workBook = null;
            _workSheet = null;
            _cells = null;
            _rows = null;
            _columns = null;
        }
    }
}
