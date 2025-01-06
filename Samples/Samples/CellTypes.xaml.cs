using AlphaX.WPF.Sheets;
using AlphaX.WPF.Sheets.CellTypes;
using System;
using System.Windows.Controls;

namespace AlphaXSpreadSamplesExplorer.Samples
{
    /// <summary>
    /// Interaction logic for CellTypes.xaml
    /// </summary>
    public partial class CellTypes : UserControl
    {
        public CellTypes()
        {
            InitializeComponent();
            SetupSheet(spread.SheetViews.ActiveSheetView);
        }

        private void SetupSheet(IAlphaXSheetView sheetView)
        {
            var worksheet = sheetView.WorkSheet;

            worksheet.Columns[0].Locked = true;
            worksheet.Columns[0].CellType = new TextCellType();
            worksheet.Columns[1].Locked = true;
            worksheet.Columns[1].CellType = new CheckBoxCellType() { IsThreeState = true };
            worksheet.Columns[2].CellType = new NumberCellType() { Format = "#,##" };
            worksheet.Columns[3].Locked = true;
            worksheet.Columns[3].CellType = new DateTimeCellType();
            worksheet.Columns[4].Locked = true;
            worksheet.Columns[4].CellType = new ButtonCellType() { Text = "Button" };

            Random rnd = new Random();
            for (int row = 0; row < 50; row++)
            {
                worksheet.Cells[row, 0].Value = $"Text {row + 1}";
                worksheet.Cells[row, 1].Value = rnd.Next(1, 10) % 2 == 0 ? true : rnd.NextDouble() < 0.5 ? false : (bool?)null;
                worksheet.Cells[row, 2].Value = rnd.Next(10000, 20000);
                worksheet.Cells[row, 3].Value = new DateTime(rnd.Next(2001, 2020), rnd.Next(1, 12), rnd.Next(1, 28));
            }
        }
    }
}
