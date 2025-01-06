using AlphaX.Sheets;
using AlphaX.WPF.Sheets.Components;
using AlphaX.WPF.Sheets.Rendering;
using AlphaX.WPF.Sheets.UI.Managers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets
{
    /// <summary>
    /// Represents view for a workbook.
    /// </summary>
    public class AlphaXSpread : Control, IDisposable
    {
        internal const double GridLineThickness = 0.25;

        #region Dependency Properties
        public static readonly DependencyProperty ScrollBarStyleProperty;
        public static readonly DependencyProperty ScrollModeProperty;
        public static readonly DependencyProperty SelectionBackgroundProperty;
        public static readonly DependencyProperty GridLineBrushProperty;
        public static readonly DependencyProperty SelectionBorderBrushProperty;
        public static readonly DependencyProperty AllowRowResizeProperty;
        public static readonly DependencyProperty AllowColumnResizeProperty;
        public static readonly DependencyProperty ShowFormulaSuggestionsProperty;
        public static readonly DependencyProperty SheetTabsVisibilityProperty;

        static AlphaXSpread()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AlphaXSpread), new FrameworkPropertyMetadata(typeof(AlphaXSpread)));
            ScrollModeProperty = DependencyProperty.Register("ScrollMode", typeof(SheetScrollMode), typeof(AlphaXSpread),
                new PropertyMetadata(SheetScrollMode.Item));
            SelectionBackgroundProperty = DependencyProperty.Register("SelectionBackground", typeof(Brush), typeof(AlphaXSpread),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(50, 25, 25, 25))));
            GridLineBrushProperty = DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(AlphaXSpread), 
                new PropertyMetadata(OnGridLineBrushChanged));
            SelectionBorderBrushProperty = DependencyProperty.Register("SelectionBorderBrush", typeof(Brush), typeof(AlphaXSpread),
                new PropertyMetadata(OnSelectionBorderBrushChanged));
            AllowRowResizeProperty = DependencyProperty.Register("AllowRowResize", typeof(bool), typeof(AlphaXSpread), new PropertyMetadata(true));
            AllowColumnResizeProperty =
            DependencyProperty.Register("AllowColumnResize", typeof(bool), typeof(AlphaXSpread), new PropertyMetadata(true));
            ShowFormulaSuggestionsProperty =
            DependencyProperty.Register("ShowFormulaSuggestions", typeof(bool), typeof(AlphaXSpread), new PropertyMetadata(true));
            SheetTabsVisibilityProperty =
            DependencyProperty.Register("SheetTabsVisibility", typeof(Visibility), typeof(AlphaXSpread), new PropertyMetadata(Visibility.Visible));
            ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("/AlphaX.WPF.Sheets;component/Themes/ScrollBarStyle.xaml", UriKind.Relative));
            ScrollBarStyleProperty =
            DependencyProperty.Register("ScrollBarStyle", typeof(System.Windows.Style), typeof(AlphaXSpread), new PropertyMetadata((System.Windows.Style)res["ScrollBarStyle"]));
        }

        /// <summary>
        /// Gets or sets scrollbar style.
        /// </summary>
        public System.Windows.Style ScrollBarStyle
        {
            get { return (System.Windows.Style)GetValue(ScrollBarStyleProperty); }
            set { SetValue(ScrollBarStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the sheet tabs are visible.
        /// </summary>
        public Visibility SheetTabsVisibility
        {
            get { return (Visibility)GetValue(SheetTabsVisibilityProperty); }
            set { SetValue(SheetTabsVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the formula suggestion is enabled.
        /// </summary>
        public bool ShowFormulaSuggestions
        {
            get { return (bool)GetValue(ShowFormulaSuggestionsProperty); }
            set { SetValue(ShowFormulaSuggestionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the columns can be resized.
        /// </summary>
        public bool AllowColumnResize
        {
            get { return (bool)GetValue(AllowColumnResizeProperty); }
            set { SetValue(AllowColumnResizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the rows can be resized.
        /// </summary>
        public bool AllowRowResize
        {
            get { return (bool)GetValue(AllowRowResizeProperty); }
            set { SetValue(AllowRowResizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scroll mode.
        /// </summary>
        public SheetScrollMode ScrollMode
        {
            get { return (SheetScrollMode)GetValue(ScrollModeProperty); }
            set { SetValue(ScrollModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection background.
        /// </summary>
        public Brush SelectionBackground
        {
            get { return (Brush)GetValue(SelectionBackgroundProperty); }
            set { SetValue(SelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the grid line brush.
        /// </summary>
        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection border brush.
        /// </summary>
        public Brush SelectionBorderBrush
        {
            get { return (Brush)GetValue(SelectionBorderBrushProperty); }
            set { SetValue(SelectionBorderBrushProperty, value); }
        }
        #endregion

        private AlphaXSheetTabControl _tabControl;

        /// <summary>
        /// Fires when cell selection changes.
        /// </summary>
        public event EventHandler<CellsSelectionEventArgs> CellsSelectionChanged;
        /// <summary>
        /// Fires on calculation error.
        /// </summary>
        public event EventHandler<CalcErrorEventArgs> CalculationError;
        internal RenderEngine RenderEngine { get; }
        internal AlphaXSheetViewPane SheetViewPane { get; }
        internal AlphaXSheetTabControl SheetTabControl => _tabControl;
        internal AlphaXFormulaTextBox FormulaTextBox { get; set; }
        internal Pen GridLinePen { get; private set; }
        internal Pen SelectionBorderPen { get; private set; }
        internal double PixelPerDip { get; set; }
        /// <summary>
        /// Gets the undo/redo manager.
        /// </summary>
        public UndoRedoManager UndoRedoManager { get; private set; }
        /// <summary>
        /// Gets the workbook.
        /// </summary>
        public WorkBook WorkBook { get; }
        /// <summary>
        /// Gets the editing manager.
        /// </summary>
        public IEditingManager EditingManager { get; }
        /// <summary>
        /// Gets the selection manager.
        /// </summary>
        public ISelectionManager SelectionManager { get; }
        /// <summary>
        /// Gets the sheetview collection.
        /// </summary>
        public SheetViewCollection SheetViews { get; }

        #region ctor
        public AlphaXSpread()
        {
            WorkBook = new WorkBook("Book1", new UIUpdateProvider(this));
            UndoRedoManager = new UndoRedoManager(this);
            AddDefaultStyles(WorkBook);
            SheetViews = new SheetViewCollection(this);
            RenderEngine = new RenderEngine();
            SheetViewPane = new AlphaXSheetViewPane(this);
            ScrollMode = SheetScrollMode.Item;
            SelectionBorderBrush = Brushes.Green;
            BorderBrush = Brushes.Black;
            Background = Brushes.Transparent;
            SnapsToDevicePixels = true;
            BorderThickness = new Thickness(0.75);
            GridLineBrush = Brushes.Gray;
            PixelPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            var workSheet = WorkBook.WorkSheets.AddSheet("Sheet1");
            WorkBook.WorkSheets.ActiveSheet = workSheet;
            EditingManager = new EditingManager(this);
            SelectionManager = new SelectionManager(this);
            SelectionManager.SelectCell(0, 0);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Hittest the spread at specific point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public SpreadHitTestResult HitTest(Point point)
        {
            if(SheetViews.ActiveSheetView != null)
            {
                var activeSheetView = SheetViews.ActiveSheetView.As<AlphaXSheetView>();
                var columnHeaderHeight = activeSheetView.GetColumnHeaderHeight();
                var rowHeaderWidth = activeSheetView.GetRowHeaderWidth();

                // Row headers hit test
                if (point.X >= 0 && point.X < rowHeaderWidth && point.Y >= columnHeaderHeight && point.Y < SheetViewPane.RenderSize.Height)
                    return SheetViewPane.RowHeadersRegion.HitTest(SheetViewPane.TranslatePoint(point, SheetViewPane.RowHeadersRegion));

                // Cells hit test
                if (point.X >= rowHeaderWidth && point.Y >= columnHeaderHeight && point.X < SheetViewPane.RenderSize.Width && point.Y < SheetViewPane.RenderSize.Height)
                    return SheetViewPane.CellsRegion.HitTest(SheetViewPane.TranslatePoint(point, SheetViewPane.CellsRegion));

                // Column headers hit test
                if (point.X >= rowHeaderWidth && point.Y >= 0 && point.Y < columnHeaderHeight && point.X < SheetViewPane.RenderSize.Width)
                    return SheetViewPane.ColumnHeadersRegion.HitTest(SheetViewPane.TranslatePoint(point, SheetViewPane.ColumnHeadersRegion));

                if (point.X < rowHeaderWidth && point.Y < columnHeaderHeight)
                    return SheetViewPane.TopLeftRegion.HitTest(SheetViewPane.TranslatePoint(point, SheetViewPane.TopLeftRegion));

                return null;
            }

            return null;
        }

        /// <summary>
        /// Scrolls to specific row.
        /// </summary>
        /// <param name="sheetView"></param>
        /// <param name="row"></param>
        public void ScrollToRow(IAlphaXSheetView sheetView, int row)
        {
            var workSheet = sheetView.WorkSheet;
            SheetTabControl.VScrollBar.Value = workSheet.Rows.GetLocation(row);
        }

        /// <summary>
        /// Scrolls to specific column.
        /// </summary>
        /// <param name="sheetView"></param>
        /// <param name="column"></param>
        public void ScrollToColumn(IAlphaXSheetView sheetView, int column)
        {
            var workSheet = sheetView.WorkSheet;
            SheetTabControl.HScrollBar.Value = workSheet.Columns.GetLocation(column);
        }
      
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the extent sheet size.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private Size GetSheetSize(IWorkSheet sheet)
        {
            var columns = sheet.Columns.As<Columns>();
            var rows = sheet.Rows.As<Rows>();
            double width = columns.GetLocation(sheet.ColumnCount - 1) + columns.GetColumnWidth(sheet.ColumnCount - 1);
            double height = rows.GetLocation(sheet.RowCount - 1) + rows.GetRowHeight(sheet.RowCount - 1);
            return new Size(width, height);
        }

        /// <summary>
        /// Updates the grid line pen.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        private void UpdateGridlinePen(Brush brush, double thickness)
        {
            GridLinePen = new Pen(brush, thickness);
            GridLinePen.Freeze();
        }

        /// <summary>
        /// Updates Selection border pen.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        private void UpdateSelectionBorderPen(Brush brush, double thickness)
        {
            SelectionBorderPen = new Pen(brush, thickness);
            SelectionBorderPen.Freeze();
        }

        private void AddDefaultStyles(WorkBook workBook)
        {
            var rowHeaderStyle = new Style();
            rowHeaderStyle.FontSize = 14;
            rowHeaderStyle.HorizontalAlignment = AlphaXHorizontalAlignment.Center;
            rowHeaderStyle.BackColor = AlphaX.Sheets.Drawing.Color.Gray;
            workBook.AddNamedStyle(StyleKeys.DefaultRowHeaderStyleKey, rowHeaderStyle);

            var columnHeaderStyle = new Style();
            columnHeaderStyle.FontSize = 14;
            columnHeaderStyle.HorizontalAlignment = AlphaXHorizontalAlignment.Center;
            columnHeaderStyle.BackColor = AlphaX.Sheets.Drawing.Color.Gray;
            workBook.AddNamedStyle(StyleKeys.DefaultColumnHeaderStyleKey, columnHeaderStyle);

            var sheetStyle = new Style();
            sheetStyle.BackColor =  AlphaX.Sheets.Drawing.Color.White;
            workBook.AddNamedStyle(StyleKeys.DefaultSheetStyleKey, sheetStyle);

            var topLeftStyle = new Style();
            topLeftStyle.ForeColor = AlphaX.Sheets.Drawing.Color.LightGray;
            workBook.AddNamedStyle(StyleKeys.DefaultTopLeftStyleKey, topLeftStyle);

            rowHeaderStyle.BackColor = topLeftStyle.BackColor = columnHeaderStyle.BackColor = AlphaX.Sheets.Drawing.Color.FromArgb(255, 240, 240, 240);
        }

        #endregion

        #region Internal Methods
        internal void RaiseCellsSelectionChanged(CellsSelectionEventArgs args)
        {
            CellsSelectionChanged?.Invoke(this, args);
        }

        internal void RaiseCalculationError(CalcErrorEventArgs args)
        {
            CalculationError?.Invoke(this, args);
        }
        #endregion

        #region Overrides
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var activeSheetView = SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.C:
                        activeSheetView.CopyToClipboard();
                        break;

                    case Key.A:
                        SelectionManager.SelectRange(activeSheetView.WorkSheet.Cells.AsCellRange());
                        break;

                    case Key.V:
                        activeSheetView.PasteFromClipboard();
                        break;

                    case Key.Y:
                        if(!EditingManager.IsEditing)
                            UndoRedoManager.Redo();
                        break;

                    case Key.Z:
                        if (!EditingManager.IsEditing)
                            UndoRedoManager.Undo();
                        break;
                }
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            var activeSheetView = SheetViews.ActiveSheetView.As<AlphaXSheetView>();

            switch (activeSheetView.MouseWheelScrollDirection)
            {
                case MouseWheelScrollDirection.Vertical:
                    if (_tabControl.VScrollBar == null)
                        return;
                    _tabControl.VScrollBar.Value += -e.Delta / 2;
                    activeSheetView.Invalidate(true, false, true, false);
                    break;

                case MouseWheelScrollDirection.Horizontal:
                    if (_tabControl.HScrollBar == null)
                        return;
                    _tabControl.HScrollBar.Value += -e.Delta / 2;
                    activeSheetView.Invalidate(false, true, true, false);
                    break;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (sizeInfo.PreviousSize == sizeInfo.NewSize)
                return;

            SheetTabControl.UpdateScrollbars();
            var activeSheetView = SheetViews.ActiveSheetView;
            activeSheetView.ScrollToHorizontalOffset(activeSheetView.ScrollPosition.X);
            activeSheetView.ScrollToVerticalOffset(activeSheetView.ScrollPosition.Y);
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
            PixelPerDip = newDpi.PixelsPerDip;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tabControl = GetTemplateChild("_sheetTabControl") as AlphaXSheetTabControl;
        }
        #endregion

        #region PropertyChanged Callbacks
        private static void OnSelectionBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spread = d as AlphaXSpread;
            if (e.NewValue != null && !e.NewValue.Equals(e.OldValue))
                spread.UpdateSelectionBorderPen(spread.SelectionBorderBrush, 1.5);
        }

        private static void OnGridLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spread = d as AlphaXSpread;
            if (e.NewValue != null && !e.NewValue.Equals(e.OldValue))
                spread.UpdateGridlinePen(spread.GridLineBrush, GridLineThickness);
        }
        #endregion

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        public void Dispose()
        {
            WorkBook.Dispose();
            SheetTabControl.Dispose();
            SheetViewPane.Dispose();
            RenderEngine.Dispose();
        }
    }
}
