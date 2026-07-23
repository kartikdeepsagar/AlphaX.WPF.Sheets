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
            worksheet.RowCount = 100000;
            worksheet.ColumnCount = 20;
            var rnd = new Random();
            var data = new object[worksheet.RowCount, worksheet.ColumnCount];
            for (int row = 0; row < worksheet.RowCount; row++)
            {
                for (int col = 0; col < worksheet.ColumnCount; col++)
                    data[row, col] = rnd.Next(100, 10000);
            }
            worksheet.Load(data);
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
