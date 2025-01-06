using AlphaX.CalcEngine.Formulas;
using AlphaX.WPF.Sheets.Components;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.UI.Editors
{
    internal class AlphaXTextBox : AlphaXEditorBase
    {
        private static Popup _suggestionPopup;
        private static Popup _descriptionPopup;
        private static TextBlock _descriptionTextBlock;
        private static SuggestionListBox _suggestionListBox;
        private Window _ownerWindow;

        public static bool IsShowingFormulaSuggestion => _suggestionPopup.IsOpen;

        static AlphaXTextBox()
        {
            _suggestionPopup = new Popup() 
            { 
                Placement = PlacementMode.Bottom,
                HorizontalOffset = 7,
                VerticalOffset = 5,
                PopupAnimation = PopupAnimation.Fade,
                AllowsTransparency = true,
                IsOpen = false 
            };

            _descriptionPopup = new Popup() 
            { 
                Placement = PlacementMode.Right, 
                IsOpen = false,
                HorizontalOffset = 10,
                AllowsTransparency = true,   
                PopupAnimation = PopupAnimation.Fade
            };

            _descriptionTextBlock = new TextBlock() { Foreground = Brushes.Black, Padding = new Thickness(2, 0, 2, 0) };
            _descriptionPopup.Child = new Border()
            {
                Child = _descriptionTextBlock,
                BorderThickness = new Thickness(0.5),
                BorderBrush = Brushes.Black,
                Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240))
            };

            _suggestionListBox = new SuggestionListBox();
            _suggestionPopup.Child = _suggestionListBox;
            _suggestionListBox.Width = 100;
            _suggestionListBox.DisplayMemberPath = "Name";
            _suggestionListBox.SelectedValuePath = "Name";
        }

       

        public AlphaXTextBox()
        {
            BorderThickness = new Thickness();
            _suggestionPopup.PlacementTarget = this;
            Unloaded += OnUnloaded;
            Loaded += OnLoaded;
        }

       

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if(_suggestionPopup.IsOpen && _suggestionListBox.Items.Count > 0)
            {
                if (e.Key == Key.Down && _suggestionListBox.SelectedIndex < _suggestionListBox.Items.Count - 1)
                    _suggestionListBox.SelectedIndex++;

                if (e.Key == Key.Up && _suggestionListBox.SelectedIndex > 0)
                    _suggestionListBox.SelectedIndex--;

                if (e.Key == Key.Tab)
                {
                    e.Handled = true;
                    Text = $"={_suggestionListBox.SelectedValue}(";
                    CaretIndex = Text.Length;
                    HidePopups();
                }
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if(SheetView.Spread.FormulaTextBox != null)
                SheetView.Spread.FormulaTextBox._txtEditor.Text = Text;
            TryShowSuggestionPopup();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            TryShowSuggestionPopup();
        }

        public void TryShowSuggestionPopup()
        {
            if (!SheetView.Spread.ShowFormulaSuggestions)
                return;

            if (Text.Length > 1 && Text.StartsWith("="))
            {
                var searchString = Text.Substring(1);
                var formulas = SheetView.Spread.WorkBook.CalcEngine.GetRegisteredFormulas();
                var searchedFormulas = formulas.Where(fx => fx.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));
                if (searchedFormulas.Count() > 0)
                {
                    _suggestionListBox.ItemsSource = searchedFormulas;
                    _suggestionPopup.IsOpen = true;
                    _suggestionListBox.SelectedIndex = -1;
                    _suggestionListBox.SelectedIndex = 0;
                }
                else
                {
                    HidePopups();
                }
            }
            else
            {
                HidePopups();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _ownerWindow = Window.GetWindow(this);
            _ownerWindow.Deactivated += OnMainWindowDeactivated;
            _ownerWindow.LocationChanged += OnWindowLocationChanged;
            _suggestionListBox.PreviewMouseLeftButtonDown += OnSuggestionListBoxMouseLeftButtonDown;
            _suggestionListBox.SelectionChanged += OnFormulaSelected;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _suggestionPopup.IsOpen = false;
            _ownerWindow.Deactivated -= OnMainWindowDeactivated;
            _ownerWindow.LocationChanged -= OnWindowLocationChanged;
            _suggestionListBox.PreviewMouseLeftButtonDown -= OnSuggestionListBoxMouseLeftButtonDown;
            _suggestionListBox.SelectionChanged -= OnFormulaSelected;
            Unloaded -= OnUnloaded;
        }

        private void OnFormulaSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedFormula = _suggestionListBox.SelectedItem as Formula;
            if (selectedFormula == null)
                return;
            var listBoxItem = _suggestionListBox.ItemContainerGenerator.ContainerFromItem(selectedFormula) as ListBoxItem;
            _descriptionPopup.PlacementTarget = listBoxItem;
            _descriptionTextBlock.Text = selectedFormula.GetDescription();
            _descriptionPopup.IsOpen = true;
        }

        private void OnWindowLocationChanged(object sender, EventArgs e)
        {
            if (!SheetView.Spread.ShowFormulaSuggestions)
                return;

            RelocatePopup();
        }

        private void RelocatePopup()
        {
            var offset = _suggestionPopup.HorizontalOffset;
            _suggestionPopup.HorizontalOffset = offset + 1;
            _suggestionPopup.HorizontalOffset = offset;

            offset = _descriptionPopup.HorizontalOffset;
            _descriptionPopup.HorizontalOffset = offset + 1;
            _descriptionPopup.HorizontalOffset = offset;
        }

        private void OnMainWindowDeactivated(object sender, EventArgs e)
        {
            HidePopups();
        }

        private void OnSuggestionListBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Text = $"={_suggestionListBox.SelectedValue}(";
                CaretIndex = Text.Length;
                HidePopups();
            }
        }

        private void HidePopups()
        {
            _suggestionPopup.IsOpen = false;
            _descriptionPopup.IsOpen = false;
        }
    }
}
