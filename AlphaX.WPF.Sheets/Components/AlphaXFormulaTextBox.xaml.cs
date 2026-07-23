using AlphaX.WPF.Sheets.Commands;
using AlphaX.WPF.Sheets.UI.Editors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlphaX.WPF.Sheets
{
    /// <summary>
    /// Interaction logic for AlphaXFormulaTextBox.xaml
    /// </summary>
    public partial class AlphaXFormulaTextBox : UserControl
    {
        public AlphaXSpread Spread
        {
            get { return (AlphaXSpread)GetValue(SpreadProperty); }
            set { SetValue(SpreadProperty, value); }
        }

        public static readonly DependencyProperty SpreadProperty =
            DependencyProperty.Register("Spread", typeof(AlphaXSpread), typeof(AlphaXFormulaTextBox), new PropertyMetadata(OnSpreadAttached));

        public ICommand CommitCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private bool _isExpanded = true;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                UpdateExpandState();
            }
        }

        public AlphaXFormulaTextBox()
        {
            InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Stretch;
            UpdateExpandState();
        }

        private void OnExpandToggleClick(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void UpdateExpandState()
        {
            if (_txtEditor == null)
                return;

            if (_isExpanded)
            {
                _txtEditor.MinHeight = 64;
                _txtEditor.MaxHeight = 64;
                _txtEditor.Height = 64;
                if (_expandIcon != null)
                    _expandIcon.Data = System.Windows.Media.Geometry.Parse("M 2 7 L 6 3 L 10 7");
                if (_btnExpand != null)
                    _btnExpand.ToolTip = "Collapse Formula Bar (Ctrl+Shift+U)";
            }
            else
            {
                _txtEditor.MinHeight = 26;
                _txtEditor.MaxHeight = 26;
                _txtEditor.Height = 26;
                if (_expandIcon != null)
                    _expandIcon.Data = System.Windows.Media.Geometry.Parse("M 2 3 L 6 7 L 10 3");
                if (_btnExpand != null)
                    _btnExpand.ToolTip = "Expand Formula Bar (Ctrl+Shift+U)";
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _txtEditor.ScrollToEnd();

            if (Spread == null || !_txtEditor.IsFocused)
                return;

            var activeSheetView = Spread.SheetViews.ActiveSheetView;

            if (Spread.EditingManager.IsEditing)
            {
                var editor = Spread.EditingManager.ActiveEditor as IEditorInfo;
                editor.SetValue(_txtEditor.Text);
                return;
            }

            Spread.EditingManager.BeginEdit(activeSheetView.ActiveRow, activeSheetView.ActiveColumn);
            (Spread.EditingManager.ActiveEditor as IEditorInfo).SetValue(_txtEditor.Text);
            _txtEditor.Focus();
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (Spread == null)
                return;

            Key key = e.Key == Key.System ? e.SystemKey : e.Key;

            if (key == Key.U && (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift)))
            {
                e.Handled = true;
                IsExpanded = !IsExpanded;
                return;
            }

            if (key == Key.Enter && (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Alt) || Keyboard.Modifiers.HasFlag(ModifierKeys.Alt)))
            {
                e.Handled = true;
                int caretIndex = _txtEditor.CaretIndex;
                string currentText = _txtEditor.Text ?? "";
                if (_txtEditor.SelectionLength > 0)
                {
                    currentText = currentText.Remove(_txtEditor.SelectionStart, _txtEditor.SelectionLength);
                    caretIndex = _txtEditor.SelectionStart;
                }
                _txtEditor.Text = currentText.Insert(caretIndex, System.Environment.NewLine);
                _txtEditor.CaretIndex = caretIndex + System.Environment.NewLine.Length;
                _txtEditor.ScrollToEnd();
                return;
            }

            if(key == Key.Enter)
            {
                e.Handled = true;
                CommitCommand.Execute(null);
                var activeSheetView = Spread.SheetViews.ActiveSheetView;
                Spread.SelectionManager.SelectCell(activeSheetView.ActiveRow + 1, activeSheetView.ActiveColumn);
            }
        }

        private void OnCellsSelectionChanged(object sender, CellsSelectionEventArgs e)
        {
            var workSheet = e.SheetView.WorkSheet;
            var cell = workSheet.Cells.GetCell(e.SheetView.ActiveRow, e.SheetView.ActiveColumn, false);

            if(cell == null)
            {
                var dataStore = workSheet.DataStore;
                var value = dataStore.GetValue(e.SheetView.ActiveRow, e.SheetView.ActiveColumn);
                _txtEditor.Text = value?.ToString();
            }
            else if(!string.IsNullOrEmpty(cell.Formula))
            {
                _txtEditor.Text = $"={cell.Formula}";
            }
            else
            {
                _txtEditor.Text = cell.Value?.ToString();
            }

            _txtEditor.ScrollToEnd();
        }

        private static void OnSpreadAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fTextBox = d as AlphaXFormulaTextBox;
            if (fTextBox == null)
                return;

            if (e.OldValue is AlphaXSpread oldSpread)
            {
                oldSpread.CellsSelectionChanged -= fTextBox.OnCellsSelectionChanged;
                if (oldSpread.FormulaTextBox == fTextBox)
                    oldSpread.FormulaTextBox = null;
            }

            if (e.NewValue is AlphaXSpread newSpread)
            {
                newSpread.FormulaTextBox = fTextBox;
                newSpread.CellsSelectionChanged += fTextBox.OnCellsSelectionChanged;
                fTextBox.CommitCommand = new CommitEditCommand(newSpread);
                fTextBox.CancelCommand = new CancelEditCommand(newSpread);
                fTextBox.DataContext = fTextBox;
            }
            else
            {
                fTextBox.CommitCommand = null;
                fTextBox.CancelCommand = null;
                fTextBox.DataContext = null;
            }
        }
    }
}
