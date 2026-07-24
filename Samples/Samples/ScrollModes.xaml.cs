using AlphaX.Sheets;
using AlphaX.Sheets.Drawing;
using System;
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
            PopulateLargeDataset();
        }

        private void PopulateLargeDataset()
        {
            var worksheet = spread.SheetViews.ActiveSheetView.WorkSheet;
            int rowCount = 50000;
            int colCount = 10;

            worksheet.RowCount = rowCount;
            worksheet.ColumnCount = colCount;

            string[] departments = { "Engineering", "Sales", "Marketing", "Finance", "Human Resources", "Operations", "Legal", "Product" };
            string[] regions = { "North America", "Europe", "Asia Pacific", "Latin America", "Middle East" };
            string[] statuses = { "Active", "Pending", "Completed", "On Hold", "Archived" };

            var rnd = new Random(42);
            var data = new object[rowCount, colCount];

            // Header labels in row 0
            string[] headers = { "ID", "Employee Ref", "Department", "Region", "Salary ($)", "Score", "Projects", "Status", "Year Joined", "Security Code" };
            for (int col = 0; col < colCount; col++)
            {
                data[0, col] = headers[col];
            }

            for (int row = 1; row < rowCount; row++)
            {
                data[row, 0] = row;
                data[row, 1] = $"EMP-{100000 + row}";
                data[row, 2] = departments[rnd.Next(departments.Length)];
                data[row, 3] = regions[rnd.Next(regions.Length)];
                data[row, 4] = rnd.Next(45000, 185000);
                data[row, 5] = Math.Round(3.0 + rnd.NextDouble() * 2.0, 1);
                data[row, 6] = rnd.Next(1, 15);
                data[row, 7] = statuses[rnd.Next(statuses.Length)];
                data[row, 8] = rnd.Next(2010, 2026);
                data[row, 9] = $"SEC-{rnd.Next(1000, 9999)}";
            }

            worksheet.Load(data);

            // Style headers with Excel Green
            string headerStyleName = "ScrollHeaderStyle";
            if (worksheet.WorkBook.GetNamedStyle(headerStyleName) == null)
            {
                var style = new AlphaX.WPF.Sheets.AlphaXStyle
                {
                    BackColor = Color.FromArgb(255, 16, 124, 65), // #107C41 Excel Green
                    ForeColor = Color.White,
                    FontWeight = AlphaX.Sheets.Drawing.FontWeight.Bold,
                    HorizontalAlignment = AlphaXHorizontalAlignment.Center
                };
                worksheet.WorkBook.AddNamedStyle(headerStyleName, style);
            }
            worksheet.Rows[0].StyleName = headerStyleName;

            worksheet.Columns[0].Width = 70;
            worksheet.Columns[1].Width = 110;
            worksheet.Columns[2].Width = 140;
            worksheet.Columns[3].Width = 130;
            worksheet.Columns[4].Width = 110;
            worksheet.Columns[5].Width = 100;
            worksheet.Columns[7].Width = 100;
        }
    }
}
