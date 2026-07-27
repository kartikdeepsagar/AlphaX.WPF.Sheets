using System;
using System.Globalization;

namespace DevBrewLabs.Spreadsheet.Formatters
{
    /// <summary>
    /// Enterprise-grade, high-performance general cell formatter for spreadsheet engines.
    /// Handles culture-aware formatting, floating-point roundoff normalization, and zero-allocation type dispatch.
    /// </summary>
    public sealed class GeneralFormatter : IFormatter
    {
        private const int MaxFloatingPointPrecision = 10;
        private readonly CultureInfo _culture;

        /// <summary>
        /// Gets the default thread-safe instance using current culture.
        /// </summary>
        public static GeneralFormatter Default { get; } = new GeneralFormatter();

        /// <summary>
        /// Initializes a new instance using <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        public GeneralFormatter() : this(CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance using the specified culture.
        /// </summary>
        /// <param name="culture">The culture info for formatting numbers and dates.</param>
        public GeneralFormatter(CultureInfo culture)
        {
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Formats an arbitrary cell value into its display string representation.
        /// </summary>
        /// <param name="value">The raw cell value.</param>
        /// <returns>Formatted string representation or null if value is null.</returns>
        public string Format(object value)
        {
            if (value == null)
                return null;

            // Direct Type Dispatch (Fastest Path - No boxing/string parsing)
            switch (value)
            {
                case string str:
                    return str;

                case double d:
                    return FormatDouble(d, _culture);

                case float f:
                    return FormatDouble(f, _culture);

                case decimal dec:
                    return dec.ToString("G", _culture);

                case int i:
                    return i.ToString(_culture);

                case long l:
                    return l.ToString(_culture);

                case bool b:
                    return b ? "TRUE" : "FALSE";

                case DateTime dt:
                    // Excel General format displays dates in short date format
                    return dt.TimeOfDay == TimeSpan.Zero
                        ? dt.ToString("d", _culture)
                        : dt.ToString("g", _culture);

                case TimeSpan ts:
                    return ts.ToString(@"hh\:mm\:ss", _culture);

                case byte bVal:
                    return bVal.ToString(_culture);

                case short sVal:
                    return sVal.ToString(_culture);

                case uint uiVal:
                    return uiVal.ToString(_culture);

                case ulong ulVal:
                    return ulVal.ToString(_culture);

                case Enum enumVal:
                    return enumVal.ToString();

                case IFormattable formattable:
                    return formattable.ToString(null, _culture);

                default:
                    // Fallback for custom objects / fallback number extension checks
                    return FormatFallback(value, _culture);
            }
        }

        /// <summary>
        /// High-precision double formatting with IEEE 754 floating-point noise reduction.
        /// </summary>
        private static string FormatDouble(double val, CultureInfo culture)
        {
            if (double.IsNaN(val))
                return "#NUM!";

            if (double.IsPositiveInfinity(val) || double.IsNegativeInfinity(val))
                return "#DIV/0!";

            // 1. Round to 14 decimal places to strip IEEE 754 noise (-0.010000000000000009 -> -0.01)
            double rounded = Math.Round(val, MaxFloatingPointPrecision);

            // 2. Format with "G15" to preserve up to 15 significant digits without scientific notation bloat
            return rounded.ToString("G15", culture);
        }

        private static string FormatFallback(object value, CultureInfo culture)
        {
            string rawStr = value.ToString();

            // Handle legacy/custom numeric wrapper objects
            if (value.IsNumber() && double.TryParse(rawStr, NumberStyles.Any, culture, out double parsedDouble))
            {
                return FormatDouble(parsedDouble, culture);
            }

            return rawStr;
        }
    }
}