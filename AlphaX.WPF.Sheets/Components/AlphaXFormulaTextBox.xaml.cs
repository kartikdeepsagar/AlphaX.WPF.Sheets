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

        public AlphaXFormulaTextBox()
        {
            InitializeComponent();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
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

            if(e.Key == Key.Enter)
            {
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
        }

        private static void OnSpreadAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fTextBox = d as AlphaXFormulaTextBox;
            var spread = e.NewValue as AlphaXSpread;

            if (spread == null)
            {
                fTextBox.DataContext = null;
                spread.FormulaTextBox = null;
                return;
            }

            spread.FormulaTextBox = fTextBox;
            spread.CellsSelectionChanged += fTextBox.OnCellsSelectionChanged;
            fTextBox.CommitCommand = new CommitEditCommand(spread);
            fTextBox.CancelCommand = new CancelEditCommand(spread);
            fTextBox.DataContext = fTextBox;
        }
    }
}
