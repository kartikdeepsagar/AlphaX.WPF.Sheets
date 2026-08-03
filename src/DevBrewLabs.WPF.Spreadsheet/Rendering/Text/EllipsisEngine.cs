namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal static class EllipsisEngine
    {
        public static int Truncate(
            string text,
            int fitCount,
            double exactTotalWidth,
            double availableWidth,
            double scaledFontSize,
            GlyphMetrics metrics,
            double pixelsPerDip,
            out double finalExactWidth)
        {
            if (metrics.EllipsisGlyph == 0)
            {
                // Cannot draw ellipsis, just truncate exactly where it fits
                finalExactWidth = exactTotalWidth;
                return fitCount;
            }

            double exactEllipsisWidth = metrics.EllipsisAdvance * scaledFontSize;

            // Backtrack to fit the ellipsis
            while (fitCount > 0)
            {
                // We use snapped X for bounds checking to match how WPF would render
                // But wait, the exact Total Width + Ellipsis Width needs to be within availableWidth
                double snappedNextX = System.Math.Round((exactTotalWidth + exactEllipsisWidth) * pixelsPerDip) / pixelsPerDip;

                if (snappedNextX <= availableWidth)
                    break;

                fitCount--;
                char c = text[fitCount];
                
                // Fast path for ASCII
                if (c < 128)
                {
                    exactTotalWidth -= metrics.AsciiAdvances[c] * scaledFontSize;
                }
                else if (metrics.CharacterToGlyphMap.TryGetValue(c, out ushort glyph))
                {
                    exactTotalWidth -= metrics.AdvanceWidthMap[glyph] * scaledFontSize;
                }
            }

            // If we couldn't even fit an ellipsis, draw nothing.
            if (fitCount == 0 && (System.Math.Round(exactEllipsisWidth * pixelsPerDip) / pixelsPerDip) > availableWidth)
            {
                finalExactWidth = 0;
                return 0;
            }

            finalExactWidth = exactTotalWidth + exactEllipsisWidth;
            return fitCount;
        }
    }
}
