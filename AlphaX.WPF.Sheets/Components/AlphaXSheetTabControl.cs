using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace AlphaX.WPF.Sheets.Components
{
    public class AlphaXSheetTabControl : Control, IDisposable
    {
        static AlphaXSheetTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AlphaXSheetTabControl), new FrameworkPropertyMetadata(typeof(AlphaXSheetTabControl)));
        }

        private ScrollBar _hScrollBar;
        private ScrollBar _vScrollBar;
        private ListBox _sheetsListBox;
        private RepeatButton _nextButton;
        private RepeatButton _previousButton;
        private Button _addButton;
        private Border _sheetViewPaneBorder;
        private Grid _root;
        private bool _eventsRegistered;

        internal ScrollBar HScrollBar => _hScrollBar;
        internal ScrollBar VScrollBar => _vScrollBar;
        internal AlphaXSpread Spread { get; private set; }

        public AlphaXSheetTabControl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Spread = TemplatedParent.As<AlphaXSpread>();
            _root = GetTemplateChild("_root").As<Grid>();
            _sheetViewPaneBorder = GetTemplateChild("_sheetViewPaneBorder").As<Border>();
            _sheetViewPaneBorder.Child = Spread.SheetViewPane;
            _sheetViewPaneBorder.BorderBrush = Spread.BorderBrush;
            _sheetViewPaneBorder.BorderThickness = new Thickness(0, 0, 0.75, 0.75);
            _hScrollBar = GetTemplateChild("_hScrollBar").As<ScrollBar>();
            _vScrollBar = GetTemplateChild("_vScrollBar").As<ScrollBar>();
            _sheetsListBox = GetTemplateChild("_sheetsListBox").As<ListBox>();
            _previousButton = GetTemplateChild("_btnPrevious").As<RepeatButton>();
            _nextButton = GetTemplateChild("_btnNext").As<RepeatButton>();
            _addButton = GetTemplateChild("_btnAddSheet").As<Button>();
            RegisterInternalEventHandlers();
            _sheetsListBox.ItemsSource = Spread.SheetViews;
            _sheetsListBox.SelectedIndex = 0;
        }

        public void DisplayActiveSheet()
        {
            var sheetView = Spread.SheetViews.ActiveSheetView.As<AlphaXSheetView>();
            Spread.SheetViewPane.AttachSheet(sheetView);
            Spread.RenderEngine.SetRenderSheet(sheetView);
            UpdateScrollbars();
            _hScrollBar.Value = sheetView.ScrollPosition.X;
            _vScrollBar.Value = sheetView.ScrollPosition.Y;
            sheetView.ScrollToHorizontalOffset(sheetView.ScrollPosition.X);
            sheetView.ScrollToVerticalOffset(sheetView.ScrollPosition.Y);
        }

        private void OnAddSheetClick(object sender, RoutedEventArgs e)
        {
            Spread.WorkBook.WorkSheets.AddSheet($"Sheet{Spread.WorkBook.WorkSheets.Count + 1}");
            _sheetsListBox.SelectedIndex = _sheetsListBox.Items.Count - 1;
        }

        private void OnSheetSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Spread.EditingManager.IsEditing)
                Spread.EditingManager.EndEdit(true);

            var sheetView = _sheetsListBox.SelectedItem.As<AlphaXSheetView>();
            Spread.WorkBook.WorkSheets.ActiveSheet = sheetView.WorkSheet;
            DisplayActiveSheet();
        }

        private void OnNextSheetClick(object sender, RoutedEventArgs e)
        {
            if (_sheetsListBox.SelectedIndex <= _sheetsListBox.Items.Count - 1)
            {
                _sheetsListBox.SelectedIndex++;
            }
        }

        private void OnPreviousSheetClick(object sender, RoutedEventArgs e)
        {
            if (_sheetsListBox.SelectedIndex > 0)
            {
                _sheetsListBox.SelectedIndex--;
            }
        }

        /// <summary>
        /// Register internal event handlers
        /// </summary>
        private void RegisterInternalEventHandlers()
        {
            if (_eventsRegistered)
                return;

            WeakEventManager<ScrollBar, RoutedPropertyChangedEventArgs<double>>.AddHandler(_hScrollBar, "ValueChanged", OnHorizontalScrollBarValueChanged);
            WeakEventManager<ScrollBar, RoutedPropertyChangedEventArgs<double>>.AddHandler(_vScrollBar, "ValueChanged", OnVerticalScrollBarValueChanged);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(_addButton, "Click", OnAddSheetClick);
            WeakEventManager<RepeatButton, RoutedEventArgs>.AddHandler(_nextButton, "Click", OnNextSheetClick);
            WeakEventManager<RepeatButton, RoutedEventArgs>.AddHandler(_previousButton, "Click", OnPreviousSheetClick);
            WeakEventManager<ListBox, SelectionChangedEventArgs>.AddHandler(_sheetsListBox, "SelectionChanged", OnSheetSelectionChanged);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                WeakEventManager<Thumb, DragCompletedEventArgs>.AddHandler(_hScrollBar.Track.Thumb, "DragCompleted", OnHorizontalScrollDragCompleted);
                WeakEventManager<Thumb, DragCompletedEventArgs>.AddHandler(_vScrollBar.Track.Thumb, "DragCompleted", OnVerticalScrollDragCompleted);
            }), DispatcherPriority.Loaded);

            _eventsRegistered = true;
        }

        /// <summary>
        /// Unregister internal event handlers.
        /// </summary>
        private void UnRegisterInternalEventHandlers()
        {
            if (!_eventsRegistered)
                return;

            WeakEventManager<ScrollBar, RoutedPropertyChangedEventArgs<double>>.RemoveHandler(_hScrollBar, "ValueChanged", OnHorizontalScrollBarValueChanged);
            WeakEventManager<Thumb, DragCompletedEventArgs>.RemoveHandler(_hScrollBar.Track.Thumb, "DragCompleted", OnHorizontalScrollDragCompleted);
            WeakEventManager<ScrollBar, RoutedPropertyChangedEventArgs<double>>.RemoveHandler(_vScrollBar, "ValueChanged", OnVerticalScrollBarValueChanged);
            WeakEventManager<Thumb, DragCompletedEventArgs>.RemoveHandler(_vScrollBar.Track.Thumb, "DragCompleted", OnVerticalScrollDragCompleted);
            WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(_addButton, "Click", OnAddSheetClick);
            WeakEventManager<ListBox, SelectionChangedEventArgs>.RemoveHandler(_sheetsListBox, "SelectionChanged", OnSheetSelectionChanged);
            WeakEventManager<RepeatButton, RoutedEventArgs>.RemoveHandler(_nextButton, "Click", OnNextSheetClick);
            WeakEventManager<RepeatButton, RoutedEventArgs>.RemoveHandler(_previousButton, "Click", OnPreviousSheetClick);
            _eventsRegistered = false;
        }

        /// <summary>
        /// Updates the scrollbars according to the sheet size and viewport.
        /// </summary>
        internal void UpdateScrollbars()
        {
            var sheet = Spread.SheetViews.ActiveSheetView.WorkSheet;
            var columns = sheet.Columns;
            var rows = sheet.Rows;
            _hScrollBar.ViewportSize = Spread.SheetViewPane.CellsRegion.ActualWidth;
            _hScrollBar.Maximum = _hScrollBar.Minimum = _vScrollBar.Maximum = _vScrollBar.Minimum = 0;
            var lastColumnLocation = columns.GetLocation(sheet.ColumnCount - 1);

            for (int column = sheet.ColumnCount - 1; column >= 0; column--)
            {
                var location = columns.GetLocation(column);

                if (_hScrollBar.ViewportSize != 0 && lastColumnLocation - location >= _hScrollBar.ViewportSize)
                {
                    _hScrollBar.Maximum = columns.GetLocation(column + 3);
                    break;
                }
            }

            _vScrollBar.ViewportSize = Spread.SheetViewPane.CellsRegion.ActualHeight;
            var lastRowLocation = rows.GetLocation(sheet.RowCount - 1);

            for (int row = sheet.RowCount - 1; row >= 0; row--)
            {
                var location = rows.GetLocation(row);

                if (_vScrollBar.ViewportSize != 0 && lastRowLocation - location >= _vScrollBar.ViewportSize)
                {
                    _vScrollBar.Maximum = rows.GetLocation(row + 3);
                    break;
                }
            }

            if (_vScrollBar.Maximum == _vScrollBar.Minimum)
                _vScrollBar.Visibility = Visibility.Hidden;
            else
                _vScrollBar.Visibility = Visibility.Visible;

            if (_hScrollBar.Maximum == _hScrollBar.Minimum)
                _hScrollBar.Visibility = Visibility.Hidden;
            else
                _hScrollBar.Visibility = Visibility.Visible;
        }

        #region Scrolling
        private void OnVerticalScrollBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_vScrollBar.Track.Thumb.IsDragging && Spread.ScrollMode == SheetScrollMode.Deferred)
                return;

            Spread.SheetViews.ActiveSheetView.ScrollToVerticalOffset(e.NewValue);
        }

        private void OnHorizontalScrollBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hScrollBar.Track.Thumb.IsDragging && Spread.ScrollMode == SheetScrollMode.Deferred)
                return;

            Spread.SheetViews.ActiveSheetView.ScrollToHorizontalOffset(e.NewValue);
        }

        private void OnHorizontalScrollDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (Spread.ScrollMode == SheetScrollMode.Deferred)
            {
                Spread.SheetViews.ActiveSheetView.ScrollToHorizontalOffset(_hScrollBar.Value);
            }
        }

        private void OnVerticalScrollDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (Spread.ScrollMode == SheetScrollMode.Deferred)
            {
                Spread.SheetViews.ActiveSheetView.ScrollToVerticalOffset(_vScrollBar.Value);
            }
        }
        #endregion

        public void Dispose()
        {
            UnRegisterInternalEventHandlers();
            _sheetsListBox.ItemsSource = null;
            _root.Children.Clear();
            _hScrollBar = null;
            _vScrollBar = null;
            _sheetsListBox = null;
            _nextButton = null;
            _previousButton = null;
            _addButton = null;
            _sheetViewPaneBorder = null;
            _root = null;
        }
    }
}
