using System.Runtime.CompilerServices;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal static class CharacterAnalyzer
    {
        // We reject characters that require complex shaping, bidirectional (RTL) layout,
        // combining marks, or surrogate pairs (emojis).
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSupported(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                // Allow newline, carriage return, and tab
                if (c == '\n' || c == '\r' || c == '\t')
                    continue;

                // Reject other control characters
                if (char.IsControl(c))
                    return false;

                // Combining Diacritical Marks (0300–036F)
                if (c >= '\u0300' && c <= '\u036F')
                    return false;

                // Hebrew (0590-05FF), Arabic (0600-06FF), Syriac (0700-074F), Arabic Supplement (0750-077F)
                if (c >= '\u0590' && c <= '\u077F')
                    return false;

                // Devanagari, Bengali, etc. (0900-0DFF) which require complex shaping
                if (c >= '\u0900' && c <= '\u0DFF')
                    return false;

                // Surrogate pairs (High surrogates D800-DBFF, Low surrogates DC00-DFFF)
                // This essentially blocks all SMP characters (like Emojis)
                if (char.IsSurrogate(c))
                    return false;
            }

            return true;
        }
    }
}
