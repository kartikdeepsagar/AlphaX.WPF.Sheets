using System;
using System.Windows;
using System.Windows.Controls;
using DevBrewLabs.Spreadsheet;

namespace SpreadsheetSampleExplorer
{
    /// <summary>
    /// Interaction logic for Testing.xaml
    /// </summary>Row
    public partial class Testing : UserControl
    {
        public Testing()
        {
            InitializeComponent();
            spread.MouseDoubleClick += Spread_MouseDoubleClick;
            var worksheet = spread.SheetViews.ActiveSheetView.WorkSheet;
            worksheet.AllowMultiLineText = true;
            spread.ScrollMode = DevBrewLabs.WPF.Spreadsheet.SheetScrollMode.Pixel;
        }

        private void Spread_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var hitTest = spread.HitTest(e.GetPosition(spread));
            if (hitTest != null && hitTest.Element == DevBrewLabs.WPF.Spreadsheet.VisualElement.ColumnHeader)
            {
                spread.SheetViews.ActiveSheetView.AutoSizeColumn(hitTest.Column);
            }
        }

        private void Testing_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
