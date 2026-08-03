using System.Collections.Generic;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal class GlyphMetrics
    {
        public GlyphTypeface GlyphTypeface { get; }
        public double Baseline { get; }
        public double Height { get; }

        public IDictionary<int, ushort> CharacterToGlyphMap { get; }
        public IDictionary<ushort, double> AdvanceWidthMap { get; }

        // Fast path arrays for ASCII (0-127)
        public ushort[] AsciiGlyphs { get; }
        public double[] AsciiAdvances { get; }

        public ushort EllipsisGlyph { get; }
        public double EllipsisAdvance { get; }

        public ushort ReplacementGlyph { get; }
        public double ReplacementAdvance { get; }

        public GlyphMetrics(GlyphTypeface glyphTypeface)
        {
            GlyphTypeface = glyphTypeface;
            Baseline = glyphTypeface.Baseline;
            Height = glyphTypeface.Height;
            CharacterToGlyphMap = glyphTypeface.CharacterToGlyphMap;
            AdvanceWidthMap = glyphTypeface.AdvanceWidths;

            AsciiGlyphs = new ushort[128];
            AsciiAdvances = new double[128];

            for (int i = 0; i < 128; i++)
            {
                if (CharacterToGlyphMap.TryGetValue(i, out ushort glyph))
                {
                    AsciiGlyphs[i] = glyph;
                    AsciiAdvances[i] = AdvanceWidthMap[glyph];
                }
            }

            if (CharacterToGlyphMap.TryGetValue('\u2026', out ushort ellipsisGlyph))
            {
                EllipsisGlyph = ellipsisGlyph;
                EllipsisAdvance = AdvanceWidthMap[ellipsisGlyph];
            }

            // Fallback replacement character (e.g. '?')
            if (CharacterToGlyphMap.TryGetValue('?', out ushort replacementGlyph))
            {
                ReplacementGlyph = replacementGlyph;
                ReplacementAdvance = AdvanceWidthMap[replacementGlyph];
            }
        }
    }
}
