using AlphaX.Sheets;
using AlphaX.WPF.Sheets.Rendering;
using AlphaX.WPF.Sheets.UI;
using System;
using System.Text;
using System.Windows;
using System.Collections.Generic;

namespace AlphaX.WPF.Sheets
{
    internal class AlphaXSheetView : IAlphaXSheetView
    {
        private HeadersVisibility _headersVisibility;
        private ViewPort _viewPort;
        private WorkSheet _workSheet;
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
                        Invalidate();
                    }
                }
            }
        }
        public IViewPort ViewPort => _viewPort;
        public Point ScrollPosition { get; private set; }
        public SelectionMode SelectionMode { get; set; }
        public MouseWheelScrollDirection MouseWheelScrollDirection { get; set; }
        public AlphaXSpread Spread { get; }
        public int ActiveRow { get; internal set; }
        public int ActiveColumn { get; internal set; }
        public CellRange Selection { get; }
        public WorkSheet WorkSheet => _workSheet;
        #endregion

        public AlphaXSheetView(AlphaXSpread spread, WorkSheet worksheet)
        {
            Spread = spread;
            _workSheet = worksheet;
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

        public void Invalidate(bool rowHeaders = true, bool columnHeaders = true, bool cells = true, bool topLeft = true)
        {
            var pane = Spread.SheetViewPane;

            pane.Draw(rowHeaders, columnHeaders, cells, cells, topLeft);

            if (cells)
            {
                var interactionLayer = pane.CellsRegion.GetInteractionLayer();
                if (interactionLayer != null)
                    interactionLayer.InvalidateVisual();
            }

            if (rowHeaders)
            {
                var interactionLayer = pane.RowHeadersRegion.GetInteractionLayer();
                if (interactionLayer != null)
                    interactionLayer.InvalidateVisual();
            }

            if (columnHeaders)
            {
                var interactionLayer = pane.ColumnHeadersRegion.GetInteractionLayer();
                if (interactionLayer != null)
                    interactionLayer.InvalidateVisual();
            }

            if (topLeft)
                pane.TopLeftRegion.InvalidateVisual();
        }

        public void InvalidateCellRange(CellRange range)
        {
            range = _viewPort.ShrinkRangeToViewPort(range);
            if (range.IsValid)
                InvalidateCellRange(range.TopRow, range.LeftColumn, range.BottomRow, range.RightColumn);
        }

        public void InvalidateCellRange(int topRow, int leftCol, int bottomRow, int rightCol)
        {
            var pane = Spread.SheetViewPane;
            pane.DrawRange(topRow, leftCol, bottomRow, rightCol);
        }

        public void ScrollToHorizontalOffset(double offset)
        {
            double delta = offset - ScrollPosition.X;
            ScrollPosition = new Point(offset, ScrollPosition.Y);
            _viewPort.CalculateLeftColumn(delta);
            _viewPort.CalculateVisibleRange();
            Invalidate(false, true, true);
        }

        public void ScrollToVerticalOffset(double offset)
        {
            double delta = offset - ScrollPosition.Y;
            ScrollPosition = new Point(ScrollPosition.X, offset);
            _viewPort.CalculateTopRow(delta);
            _viewPort.CalculateVisibleRange();
            Invalidate(true, false, true);
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
            var sheetColumn = ((Columns)WorkSheet.Columns).GetItem(column, false);
            var width = 0;
            var cellValues = ((Cells)WorkSheet.Cells).GetCellValues(column);

            foreach(var cellValue in cellValues)
            {
                if(cellValue.Value != null)
                {
                    var style = ((WorkBook)WorkSheet.WorkBook).PickStyle(_cells.GetCell(cellValue.Key, column, false), sheetColumn, _rows.GetItem(cellValue.Key, false));
                    if (style == null)
                        style = WorkSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey).GetWpfStyle();
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
            Invalidate();
        }
    }
}
