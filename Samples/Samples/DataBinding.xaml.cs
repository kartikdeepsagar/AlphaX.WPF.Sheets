using AlphaXSpreadSamplesExplorer.Data;
using AlphaX.Sheets.Data;
using AlphaX.WPF.Sheets;
using System;
using System.Linq;
using System.Windows.Controls;

namespace AlphaXSpreadSamplesExplorer.Samples
{
    /// <summary>
    /// Interaction logic for CollectionDataBinding.xaml
    /// </summary>
    public partial class DataBinding : UserControl
    {
        public DataBinding()
        {
            InitializeComponent();
            SetupListBinding(spread.SheetViews.ActiveSheetView);
            SetuDataTableBinding(spread2.SheetViews.ActiveSheetView);
        }

        private void SetuDataTableBinding(IAlphaXSheetView sheetView)
        {
            var customers = DataSource.GetCustomersTable();
            var worksheet = sheetView.WorkSheet;
            worksheet.DataSource = customers;
            worksheet.Columns[0].DataMap = new DataColumnDataMap("Id");
            worksheet.Columns[1].DataMap = new DataColumnDataMap("Age");
            worksheet.Columns[2].DataMap = new DataColumnDataMap("FirstName");
            worksheet.Columns[3].DataMap = new DataColumnDataMap("LastName");
            worksheet.Columns[4].DataMap = new DataColumnDataMap("Gender");
            worksheet.Columns[5].DataMap = new DataColumnDataMap("Email");
            worksheet.Columns[5].Width = 200;
            worksheet.Columns[6].DataMap = new DataColumnDataMap("Phone");
            worksheet.Columns[6].Width = 100;
        }

        private void SetupListBinding(IAlphaXSheetView sheetView)
        {
            var customers = DataSource.GetCustomers().Take(100).ToList();
            var worksheet = sheetView.WorkSheet;
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
