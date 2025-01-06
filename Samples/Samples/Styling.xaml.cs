using AlphaXSpreadSamplesExplorer.Data;
using AlphaX.Sheets;
using AlphaX.Sheets.Data;
using AlphaX.Sheets.Drawing;
using AlphaX.WPF.Sheets;
using System;
using System.Linq;
using System.Windows.Controls;

namespace AlphaXSpreadSamplesExplorer.Samples
{
    /// <summary>
    /// Interaction logic for Styling.xaml
    /// </summary>
    public partial class Styling : UserControl
    {
        public Styling()
        {
            InitializeComponent();
            Setup(spread1, SetupSpread1Style);
            Setup(spread2, SetupSpread2Style);
            Setup(spread3, SetupSpread3Style);
            Setup(spread4, SetupSpread4Style);
        }

        private void Setup(AlphaXSpread spread, Action<IAlphaXSheetView> styleAction)
        {
            var sheet = spread.WorkBook.WorkSheets.GetSheet(0);
            styleAction(spread.SheetViews.GetSheetView(sheet));
            SetupSheetDataSource(sheet);
            spread.WorkBook.WorkSheets.SheetAdded += (s, e) =>
            {
                styleAction(spread.SheetViews.GetSheetView(e.WorkSheet));
            };
        }

        private void SetupSpread1Style(IAlphaXSheetView sheetView)
        {
            var worksheet = sheetView.WorkSheet;

            // Headers Styling
            string headerStyleName = "HeaderStyle";
            var headerStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(headerStyleName, headerStyle);
            headerStyle.BackColor = Color.White;
            headerStyle.ForeColor = Color.Black;
            headerStyle.HorizontalAlignment = AlphaXHorizontalAlignment.Center;
            worksheet.ColumnHeaders.Rows[0].StyleName = headerStyleName;
            worksheet.RowHeaders.Columns[0].StyleName = headerStyleName;
            worksheet.TopLeft.StyleName = headerStyleName;

            // Alternating Row Styling
            string altRowStyle = "AlternatingStyle";
            var rowStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(altRowStyle, rowStyle);
            rowStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            rowStyle.ForeColor = Color.Black;

            for (int row = 0; row < worksheet.RowCount; row++)
            {
                if (row % 2 != 0)
                {
                    worksheet.Rows[row].StyleName = altRowStyle;
                }
            }
        }

        private void SetupSpread2Style(IAlphaXSheetView sheetView)
        {
            var worksheet = sheetView.WorkSheet;

            // Headers Styling
            string headerStyleName = "HeaderStyle";
            var headerStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(headerStyleName, headerStyle);
            headerStyle.BackColor = Color.FromArgb(255, 131, 84, 139);
            headerStyle.ForeColor = Color.White;
            headerStyle.HorizontalAlignment = AlphaXHorizontalAlignment.Center;
            worksheet.ColumnHeaders.Rows[0].StyleName = headerStyleName;
            worksheet.RowHeaders.Columns[0].StyleName = headerStyleName;
            worksheet.TopLeft.StyleName = headerStyleName;

            // Alternating Row Styling
            string altRowStyle = "AlternatingStyle";
            var rowStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(altRowStyle, rowStyle);
            rowStyle.BackColor = Color.FromArgb(255, 238, 232, 246);
            rowStyle.ForeColor = Color.Black;

            for (int row = 0; row < worksheet.RowCount; row++)
            {
                if (row % 2 != 0)
                {
                    worksheet.Rows[row].StyleName = altRowStyle;
                }
            }
        }

        private void SetupSpread3Style(IAlphaXSheetView sheetView)
        {
            sheetView.Spread.SelectionBorderBrush = System.Windows.Media.Brushes.White;
            var worksheet = sheetView.WorkSheet;

            // Headers Styling
            string headerStyleName = "HeaderStyle";
            var headerStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(headerStyleName, headerStyle);
            headerStyle.BackColor = Color.FromArgb(255, 44, 62, 80);
            headerStyle.ForeColor = Color.White;
            headerStyle.HorizontalAlignment = AlphaXHorizontalAlignment.Center;
            worksheet.ColumnHeaders.Rows[0].StyleName = headerStyleName;
            worksheet.RowHeaders.Columns[0].StyleName = headerStyleName;
            worksheet.TopLeft.StyleName = headerStyleName;

            // Alternating Row Styling
            string altRowStyle = "AlternatingStyle";
            var rowStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(altRowStyle, rowStyle);
            rowStyle.BackColor = Color.FromArgb(255, 22, 160, 133);
            rowStyle.ForeColor = Color.White;

            string altRow2Style = "Alternating2Style";
            var row2Style = new Style();
            worksheet.WorkBook.AddNamedStyle(altRow2Style, row2Style);
            row2Style.BackColor = Color.FromArgb(255, 44, 62, 80);
            row2Style.ForeColor = Color.White;

            for (int row = 0; row < worksheet.RowCount; row++)
            {
                if (row % 2 != 0)
                {
                    worksheet.Rows[row].StyleName = altRow2Style;
                }
                else
                {
                    worksheet.Rows[row].StyleName = altRowStyle;
                }
            }
        }

        private void SetupSpread4Style(IAlphaXSheetView sheetView)
        {
            var worksheet = sheetView.WorkSheet;

            // Headers Styling
            string headerStyleName = "HeaderStyle";
            var headerStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(headerStyleName, headerStyle);
            headerStyle.BackColor = Color.FromArgb(255, 188, 221, 255);
            headerStyle.ForeColor = Color.Black;
            headerStyle.HorizontalAlignment = AlphaXHorizontalAlignment.Center;
            worksheet.ColumnHeaders.Rows[0].StyleName = headerStyleName;
            worksheet.RowHeaders.Columns[0].StyleName = headerStyleName;
            worksheet.TopLeft.StyleName = headerStyleName;

            // Alternating Row Styling
            string altRowStyle = "AlternatingStyle";
            var rowStyle = new Style();
            worksheet.WorkBook.AddNamedStyle(altRowStyle, rowStyle);
            rowStyle.BackColor = Color.FromArgb(255, 216, 225, 240);
            rowStyle.ForeColor = Color.Black;

            for (int row = 0; row < worksheet.RowCount; row++)
            {
                if (row % 2 != 0)
                {
                    worksheet.Rows[row].StyleName = altRowStyle;
                }
            }
        }

        private void SetupSheetDataSource(IWorkSheet worksheet)
        {
            var customers = DataSource.GetCustomers().Take(100).ToList();
            worksheet.DataSource = customers;
            worksheet.Columns[0].DataMap = new PropertyDataMap("Id");
            worksheet.Columns[1].DataMap = new PropertyDataMap("Age");
            worksheet.Columns[2].DataMap = new PropertyDataMap("FirstName");
            worksheet.Columns[3].DataMap = new PropertyDataMap("LastName");
            worksheet.Columns[4].DataMap = new PropertyDataMap("Gender");
            worksheet.Columns[5].DataMap = new PropertyDataMap("Email");
            worksheet.Columns[5].Width = 200;
            worksheet.Columns[6].DataMap = new PropertyDataMap("Phone");
            worksheet.Columns[6].Width = 100;
        }
    }
}
