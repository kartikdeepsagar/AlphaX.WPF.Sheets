namespace AlphaX.Sheets.Data
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

    public abstract class DataMap
    {
    }
}
