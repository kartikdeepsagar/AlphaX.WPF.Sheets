using System;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal static class TextMeasurer
    {
        public static void Measure(
            string text,
            double availableWidth,
            double scaledFontSize,
            GlyphMetrics metrics,
            double pixelsPerDip,
            out int fitCount,
            out double exactTotalWidth,
            out bool isTruncated)
        {
            exactTotalWidth = 0;
            fitCount = text.Length;
            isTruncated = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                double exactAdvance = 0;

                // Fast path for ASCII
                if (c < 128)
                {
                    exactAdvance = metrics.AsciiAdvances[c] * scaledFontSize;
                }
                else if (metrics.CharacterToGlyphMap.TryGetValue(c, out ushort glyph))
                {
                    exactAdvance = metrics.AdvanceWidthMap[glyph] * scaledFontSize;
                }
                else if (metrics.ReplacementGlyph != 0)
                {
                    exactAdvance = metrics.ReplacementAdvance * scaledFontSize;
                }

                double snappedNextX = Math.Round((exactTotalWidth + exactAdvance) * pixelsPerDip) / pixelsPerDip;

                if (snappedNextX > availableWidth)
                {
                    fitCount = i;
                    isTruncated = true;
                    break;
                }

                exactTotalWidth += exactAdvance;
            }
        }

        // Overload to compute total width when not constrained by bounds
        public static double MeasureWidth(
            string text,
            double scaledFontSize,
            GlyphMetrics metrics)
        {
            double exactTotalWidth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c < 128)
                {
                    exactTotalWidth += metrics.AsciiAdvances[c] * scaledFontSize;
                }
                else if (metrics.CharacterToGlyphMap.TryGetValue(c, out ushort glyph))
                {
                    exactTotalWidth += metrics.AdvanceWidthMap[glyph] * scaledFontSize;
                }
                else if (metrics.ReplacementGlyph != 0)
                {
                    exactTotalWidth += metrics.ReplacementAdvance * scaledFontSize;
                }
            }

            return exactTotalWidth;
        }
    }
}
