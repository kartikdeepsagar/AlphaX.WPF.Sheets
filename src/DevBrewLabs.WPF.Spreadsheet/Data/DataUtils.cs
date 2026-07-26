using System;

namespace DevBrewLabs.WPF.Spreadsheet
{
    internal static class DataUtils
    {
        public static object ToType(this object value, Type type)
        {
            try
            {
                return Convert.ChangeType(value, type);
            }
            catch
            {
                return value;
            }
        }
    }
}
