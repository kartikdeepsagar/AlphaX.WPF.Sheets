using System;

namespace AlphaX.Sheets.Utils
{
    /// <summary>
    /// Helper utilities for text line splitting and formatting.
    /// </summary>
    public static class TextUtils
    {
        private static readonly string[] LineSeparators = new[] { "\r\n", "\r", "\n" };

        /// <summary>
        /// Splits text into individual lines based on standard line breaks (\r\n, \r, \n).
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>An array of lines.</returns>
        public static string[] GetLines(string text)
        {
            if (text == null)
                return Array.Empty<string>();

            return text.Split(LineSeparators, StringSplitOptions.None);
        }

        /// <summary>
        /// Gets the total line count of the text.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>Line count.</returns>
        public static int GetLineCount(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return GetLines(text).Length;
        }

        /// <summary>
        /// Converts multiline text to a single line by replacing line breaks with spaces.
        /// </summary>
        /// <param name="text">The text to normalize.</param>
        /// <returns>Single line text.</returns>
        public static string NormalizeToSingleLine(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        }
    }
}
