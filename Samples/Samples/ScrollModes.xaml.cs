using System.Windows.Controls;

namespace AlphaXSpreadSamplesExplorer.Samples
{
    /// <summary>
    /// Interaction logic for ScrollModes.xaml
    /// </summary>
    public partial class ScrollModes : UserControl
    {
        public ScrollModes()
        {
            InitializeComponent();
            var worksheet = spread.SheetViews.ActiveSheetView.WorkSheet;
            for(int row = 0; row < 100; row++)
            {
                for(int col = 0; col < 10; col++)
                {
                    worksheet.Cells[row, col].Value = $"abc{col}";
                }
            }
        }
    }
}
