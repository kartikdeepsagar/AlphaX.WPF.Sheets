using GrapeCity.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeCity.WPF.Sheets.Data
{
    public class PropertyDataMap : DataMap
    {
        public string PropertyName { get; }

        public PropertyDataMap(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    public class DataColumnDataMap : DataMap
    {
        public string ColumnName { get; }

        public DataColumnDataMap(string columnName)
        {
            ColumnName = columnName;
        }
    }

    public abstract class DataMap : IDataMap
    {
    }
}
