using AlphaX.Sheets;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class RenderEngine : IRenderEngine, IDisposable
    {
        private AlphaXSheetView _sheetView;
        private WorkSheet _workSheet;
        private DispatcherProcessingDisabled _dispatcherDisabled;

        #region Renderers
        internal Renderer GridLinesRenderer { get; }
        internal Renderer CellsRenderer { get; }
        internal Renderer RowHeadersRenderer { get; }
        internal Renderer ColumnHeadersRenderer { get; }
        internal Renderer TopLeftRenderer { get; }
        #endregion

        public RenderInfo RenderInfo { get; }

        public RenderEngine()
        {
            RenderInfo = new RenderInfo();
            CellsRenderer = new CellsRenderer();
            GridLinesRenderer = new GridLinesRenderer();
            RowHeadersRenderer = new RowHeadersRenderer();
            ColumnHeadersRenderer = new ColumnHeadersRenderer();
            TopLeftRenderer = new TopLeftRenderer();
        }

        public void SetRenderSheet(AlphaXSheetView sheetView)
        {
            _sheetView = sheetView;
            _workSheet = sheetView.WorkSheet;
            CellsRenderer.SetRenderSheet(sheetView);
            GridLinesRenderer.SetRenderSheet(sheetView);
            RowHeadersRenderer.SetRenderSheet(sheetView);
            ColumnHeadersRenderer.SetRenderSheet(sheetView);
            TopLeftRenderer.SetRenderSheet(sheetView);
        }

        #region Render Begin/End
        public void BeginRender()
        {
            InitRender();
        }

        private void InitRender()
        {
            _dispatcherDisabled = Dispatcher.CurrentDispatcher.DisableProcessing();
            var viewRangeRect = _sheetView.ViewPort.GetViewRangeRect();
            RenderInfo.ViewPortGeometry = new RectangleGeometry(new Rect(0, 0, viewRangeRect.Width, viewRangeRect.Height));
        }

        public void EndRender()
        {
            CellsRenderer.EndRender();
            GridLinesRenderer.EndRender();
            RowHeadersRenderer.EndRender();
            ColumnHeadersRenderer.EndRender();
            TopLeftRenderer.EndRender();
            _dispatcherDisabled.Dispose();
        }
        #endregion

        public void DrawGridLines(int topRow, int leftCol, int bottomRow, int rightCol)
        {
            GridLinesRenderer.Render(topRow, leftCol, bottomRow, rightCol);
        }

        public void DrawCellRange(int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            CellsRenderer.Render(topRow, leftColumn, bottomRow, rightColumn);
        }

        public void DrawRowHeaderCells(int topRow, int bottomRow)
        {
            if (_sheetView.HeadersVisibility == HeadersVisibility.Row || _sheetView.HeadersVisibility == HeadersVisibility.Both)
            {
                RowHeadersRenderer.Render(topRow, 0, bottomRow, _workSheet.RowHeaders.ColumnCount - 1);
            }
        }

        public void DrawColumnHeaderCells(int leftCol, int rightCol)
        {
            if (_sheetView.HeadersVisibility == HeadersVisibility.Column || _sheetView.HeadersVisibility == HeadersVisibility.Both)
            {
                ColumnHeadersRenderer.Render(0, leftCol, _workSheet.ColumnHeaders.RowCount - 1, rightCol);
            }
        }

        public void DrawTopLeft()
        {
            if (_sheetView.HeadersVisibility == HeadersVisibility.Both)
            {
                TopLeftRenderer.Render(-1, -1, -1, -1);
            }
        }

        public void Dispose()
        {
            _workSheet = null;
            _sheetView = null;
        }
    }
}
