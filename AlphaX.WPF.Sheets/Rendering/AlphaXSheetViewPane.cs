using AlphaX.Sheets;

using AlphaX.WPF.Sheets.UI.Interaction;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class AlphaXSheetViewPane : Grid, IDisposable
    {
        private AlphaXSpread _spread;
        private AlphaXSheetView _sheetView;
        private WorkSheet _workSheet;
        private CellsInteractionLayer _cellInteractionLayer;
        private RowHeadersInteractionLayer _rowHeadersInteractionLayer;
        private ColumnHeadersInteractionLayer _columnHeadersInteractionLayer;
        private TopLeftInteractionLayer _topLeftInteractionLayer;

        public CellsRegion CellsRegion { get; private set; }
        public RowHeadersRegion RowHeadersRegion { get; private set; }
        public ColumnHeadersRegion ColumnHeadersRegion { get; private set; }
        public TopLeftRegion TopLeftRegion { get; private set; }

        public AlphaXSheetViewPane(AlphaXSpread spread)
        {
            _spread = spread;
            InitPaneLayout();
            InitRegions();
            InitInteractionLayers();
        }
  
        public void AttachSheet(AlphaXSheetView sheetView)
        {
            _sheetView = sheetView;
            _workSheet = sheetView.WorkSheet;

            CellsRegion.AttachSheet(sheetView);
            RowHeadersRegion.AttachSheet(sheetView);
            ColumnHeadersRegion.AttachSheet(sheetView);
            TopLeftRegion.AttachSheet(sheetView);

            _cellInteractionLayer.EnsureFree();
            _columnHeadersInteractionLayer.EnsureFree();
            _rowHeadersInteractionLayer.EnsureFree();
            _topLeftInteractionLayer.EnsureFree();

            _cellInteractionLayer.AttachToRegion(CellsRegion);
            _columnHeadersInteractionLayer.AttachToRegion(ColumnHeadersRegion);
            _rowHeadersInteractionLayer.AttachToRegion(RowHeadersRegion);
            _topLeftInteractionLayer.AttachToRegion(TopLeftRegion);

            var style = _spread.WorkBook.GetNamedStyle(StyleKeys.DefaultSheetStyleKey);
            CellsRegion.Background = style.As<Style>().Background;
        }

        public void DrawRange(int topRow, int leftCol, int bottomRow, int rightCol)
        {
            if (_sheetView == null)
                return;

            var viewRange = _sheetView.ViewPort.ViewRange;

            if (!viewRange.IsValid)
                return;

            _spread.RenderEngine.BeginRender();
            _spread.RenderEngine.DrawCellRange(topRow, leftCol, bottomRow, rightCol);
            _spread.RenderEngine.EndRender();
        }

        /// <summary>
        /// Draws the sheet using render engine.
        /// </summary>
        /// <param name="redraw"></param>
        /// <param name="rowHeaders"></param>
        /// <param name="columnHeaders"></param>
        /// <param name="cells"></param>
        /// <param name="gridLines"></param>
        /// <param name="topLeft"></param>
        public void Draw(bool redraw = true, bool rowHeaders = true, bool columnHeaders = true, bool cells = true, bool gridLines = true, bool topLeft = true)
        {
            if (_sheetView == null)
                return;

            var viewRange = _sheetView.ViewPort.ViewRange;

            if (!viewRange.IsValid)
                return;

            if (redraw)
                _spread.RenderEngine.BeginRenderInternal();
            else
                _spread.RenderEngine.BeginRender();
            
            if (gridLines)
                _spread.RenderEngine.DrawGridLines(viewRange.TopRow, viewRange.LeftColumn, viewRange.BottomRow, viewRange.RightColumn);

            if (columnHeaders)
                _spread.RenderEngine.DrawColumnHeaderCells(viewRange.LeftColumn, viewRange.RightColumn);

            if (rowHeaders)
                _spread.RenderEngine.DrawRowHeaderCells(viewRange.TopRow, viewRange.BottomRow);

            if (topLeft)
                _spread.RenderEngine.DrawTopLeft();

            if (cells)
                _spread.RenderEngine.DrawCellRange(viewRange.TopRow, viewRange.LeftColumn, viewRange.BottomRow, viewRange.RightColumn);

            _spread.RenderEngine.EndRender();
        }

        private void InitInteractionLayers()
        {
            _cellInteractionLayer = new CellsInteractionLayer();
            _rowHeadersInteractionLayer = new RowHeadersInteractionLayer();
            _columnHeadersInteractionLayer = new ColumnHeadersInteractionLayer();
            _topLeftInteractionLayer = new TopLeftInteractionLayer();
        }

        /// <summary>
        /// Initializes sheet render regions.
        /// </summary>
        private void InitRegions()
        {
            CellsRegion = new CellsRegion();
            Children.Add(CellsRegion);
            Grid.SetRow(CellsRegion, 1);
            Grid.SetColumn(CellsRegion, 1);

            RowHeadersRegion = new RowHeadersRegion();
            Children.Add(RowHeadersRegion);
            Grid.SetRow(RowHeadersRegion, 1);
            Grid.SetColumn(RowHeadersRegion, 0);

            ColumnHeadersRegion = new ColumnHeadersRegion();
            Children.Add(ColumnHeadersRegion);
            Grid.SetRow(ColumnHeadersRegion, 0);
            Grid.SetColumn(ColumnHeadersRegion, 1);

            TopLeftRegion = new TopLeftRegion();
            Children.Add(TopLeftRegion);
            Grid.SetRow(TopLeftRegion, 0);
            Grid.SetColumn(TopLeftRegion, 0);
        }

        /// <summary>
        /// Initializes render pane layout.
        /// </summary>
        private void InitPaneLayout()
        {
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();

            RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(0, GridUnitType.Auto),
            });
            RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(1, GridUnitType.Star)
            });
            ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(0, GridUnitType.Auto)
            });
            ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        }

        public void UpdateHeadersSize()
        {
            if (_sheetView == null)
                return;

            switch (_sheetView.HeadersVisibility)
            {
                case HeadersVisibility.Both:
                    ColumnDefinitions[0].Width = new GridLength(_workSheet.RowHeaders.Width);
                    RowDefinitions[0].Height = new GridLength(_workSheet.ColumnHeaders.Height);
                    break;

                case HeadersVisibility.Column:
                    ColumnDefinitions[0].Width = new GridLength(0);
                    RowDefinitions[0].Height = new GridLength(_workSheet.ColumnHeaders.Height);
                    break;

                case HeadersVisibility.Row:
                    ColumnDefinitions[0].Width = new GridLength(_workSheet.RowHeaders.Width);
                    RowDefinitions[0].Height = new GridLength(0);
                    break;

                case HeadersVisibility.None:
                    ColumnDefinitions[0].Width = new GridLength(0);
                    RowDefinitions[0].Height = new GridLength(0);
                    break;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateHeadersSize();
            Clip = new RectangleGeometry(new Rect(new Point(-1, -1), sizeInfo.NewSize));
        }

        public void Dispose()
        {
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            _cellInteractionLayer.Children.Clear();
            _rowHeadersInteractionLayer.Children.Clear();
            _topLeftInteractionLayer.Children.Clear();
            _columnHeadersInteractionLayer.Children.Clear();
            CellsRegion.Children.Clear();
            RowHeadersRegion.Children.Clear();
            ColumnHeadersRegion.Children.Clear();
            TopLeftRegion.Children.Clear();
            Children.Clear();
            _cellInteractionLayer = null;
            _columnHeadersInteractionLayer = null;
            _rowHeadersInteractionLayer = null;
            _topLeftInteractionLayer = null;
            CellsRegion = null;
            RowHeadersRegion = null;
            ColumnHeadersRegion = null;
            TopLeftRegion = null;
        }
    }
}
