using System;
using System.Globalization;

namespace AlphaX.Sheets.Formatters
{
    public class GeneralFormatter : IFormatter
    {
        private CultureInfo _culture;

        public GeneralFormatter()
        {
            _culture = CultureInfo.CurrentCulture;
        }

        public string Format(object value)
        {
            if (value == null)
                return null;

            if (value is string)
                return (string)value;

            if (value.IsNumber())
                return double.Parse(value.ToString(), _culture).ToString();

            if (value is DateTime)
                return DateTime.Parse(value.ToString(), _culture).ToShortDateString();

            return value.ToString();
        }
    }
}
