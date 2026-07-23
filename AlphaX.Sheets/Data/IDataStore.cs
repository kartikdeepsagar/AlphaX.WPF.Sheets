namespace AlphaX.Sheets.Data
{
    public interface IDataStore
    {
        object GetValue(int row, int column);
        void SetValue(int row, int column, object value);
    }
}
