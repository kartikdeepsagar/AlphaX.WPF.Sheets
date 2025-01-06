using AlphaXSpreadSamplesExplorer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace AlphaXSpreadSamplesExplorer.Data
{
    public static class DataSource
    {
        public static List<Customer> _customers;
        private static DataTable _customersTable;

        public static IEnumerable<Customer> GetCustomers()
        {
            if(_customers == null)
                _customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText("Data\\Customers.json"));

            return _customers;
        }

        public static DataTable GetCustomersTable()
        {
            if(_customersTable != null)
                return _customersTable;

            var customers = GetCustomers();
            _customersTable = new DataTable();
            var properties = typeof(Customer).GetProperties();
            foreach (var property in properties)
            {
                _customersTable.Columns.Add(property.Name, property.PropertyType);
            }

            foreach(var customer in customers)
            {
                var row = _customersTable.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(customer);
                }
                _customersTable.Rows.Add(row);
            }    

            return _customersTable;
        }
    }
}
