using AlphaX.Sheets.Drawing;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AlphaXSpreadSamplesExplorer.Samples
{
    /// <summary>
    /// Interaction logic for Sorting.xaml
    /// </summary>
    public partial class Sorting : UserControl
    {
        public Sorting()
        {
            InitializeComponent();

            var worksheet = spread.SheetViews.ActiveSheetView.WorkSheet;
            worksheet.RowCount = worksheet.ColumnCount = 100;
            var rnd = new Random();
            for (int row = 0; row < 100; row++)
            {
                for (int col = 0; col < 100; col++)
                    worksheet.Cells[row, col].Value = rnd.Next(100, 10000);
            }
        }

        private void OnSortAscending(object sender, RoutedEventArgs e)
        {
            var worksheet = spread.SheetViews.ActiveSheetView.WorkSheet;
            worksheet.SortRange(spread.SheetViews.ActiveSheetView.Selection, true);
        }

        private void OnSortDescending(object sender, RoutedEventArgs e)
        {
            var worksheet = spread.SheetViews.ActiveSheetView.WorkSheet;
            worksheet.SortRange(spread.SheetViews.ActiveSheetView.Selection, false);
        }
    }
}
