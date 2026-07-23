using AlphaX.Sheets;
using AlphaX.Sheets.Utils;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal static class TextRenderingExtensions
    {
        private const double TextPadding = 5;
        private const string Ellipsis = "...";

        public static void DrawText(
            this DrawingContext context,
            string text,
            Rect bounds,
            Style style,
            double pixelsPerDip,
            bool characterEllipses = false,
            bool allowMultiLineText = true)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!allowMultiLineText)
            {
                text = TextUtils.NormalizeToSingleLine(text);
            }

            string[] lines = TextUtils.GetLines(text);
            var glyphTypeface = style.GlyphTypeface;
            var glyphMap = glyphTypeface.CharacterToGlyphMap;
            var advanceMap = glyphTypeface.AdvanceWidths;
            double fontSize = style.FontSize;
            double fontLineHeight = Math.Max(fontSize + 2, Math.Round(glyphTypeface.Height * fontSize));
            double totalTextHeight = fontSize + (lines.Length - 1) * fontLineHeight;

            double startY;
            switch (style.VerticalAlignment)
            {
                case AlphaXVerticalAlignment.Top:
                    startY = Math.Round(bounds.Top + TextPadding);
                    break;
                case AlphaXVerticalAlignment.Center:
                    startY = Math.Round(bounds.Top + (bounds.Height - totalTextHeight) / 2);
                    if (startY < bounds.Top)
                        startY = bounds.Top;
                    break;
                default: // Bottom
                    startY = Math.Round(bounds.Bottom - TextPadding - totalTextHeight);
                    if (startY < bounds.Top)
                        startY = bounds.Top;
                    break;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string lineText = lines[i];
                double lineBaselineY = Math.Round(startY + fontSize + (i * fontLineHeight));

                if (string.IsNullOrEmpty(lineText))
                    continue;

                double lineWidth = ComputeTextWidth(lineText, fontSize, glyphTypeface);

                double lineX;
                switch (style.HorizontalAlignment)
                {
                    case AlphaXHorizontalAlignment.Center:
                        lineX = Math.Round(bounds.Left + (bounds.Width - lineWidth) / 2);
                        if (lineX < bounds.Left)
                            lineX = bounds.Left;
                        break;
                    case AlphaXHorizontalAlignment.Right:
                        lineX = Math.Round(bounds.Right - TextPadding - lineWidth);
                        if (lineX < bounds.Left)
                            lineX = bounds.Left;
                        break;
                    default: // Left or Auto
                        lineX = Math.Round(bounds.Left + TextPadding);
                        break;
                }

                Point renderPosition = new Point(lineX, lineBaselineY);

                ushort[] glyphIndexes = new ushort[lineText.Length];
                double[] advanceWidths = new double[lineText.Length];
                int glyphCount = 0;
                double currentLineWidth = 0;

                ushort dotGlyph = glyphMap.TryGetValue('.', out ushort dot) ? dot : (ushort)0;
                double dotWidth = advanceMap.TryGetValue(dotGlyph, out double dw) ? dw * fontSize : 0;
                double ellipsisWidth = dotWidth * 3;

                for (int j = 0; j < lineText.Length; j++)
                {
                    char c = lineText[j];
                    if (!glyphMap.TryGetValue(c, out ushort glyph))
                        continue;

                    double advance = advanceMap[glyph] * fontSize;

                    if (characterEllipses &&
                        j < lineText.Length - 1 &&
                        currentLineWidth + advance > bounds.Width - ellipsisWidth)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            glyphIndexes[glyphCount] = dotGlyph;
                            advanceWidths[glyphCount] = dotWidth;
                            glyphCount++;
                        }
                        currentLineWidth += ellipsisWidth;
                        break;
                    }

                    glyphIndexes[glyphCount] = glyph;
                    advanceWidths[glyphCount] = advance;
                    glyphCount++;
                    currentLineWidth += advance;
                }

                if (glyphCount > 0)
                {
                    if (glyphCount != glyphIndexes.Length)
                    {
                        Array.Resize(ref glyphIndexes, glyphCount);
                        Array.Resize(ref advanceWidths, glyphCount);
                    }

                    var glyphRun = new GlyphRun(
                        glyphTypeface,
                        0,
                        false,
                        fontSize,
                        (float)pixelsPerDip,
                        glyphIndexes,
                        renderPosition,
                        advanceWidths,
                        null, null, null, null, null, null);

                    context.DrawDrawing(new GlyphRunDrawing(style.Foreground, glyphRun));
                }
            }
        }

        public static int ComputeTextWidth(
            string text,
            double fontSize,
            GlyphTypeface glyphTypeface)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            var glyphMap = glyphTypeface.CharacterToGlyphMap;
            var advanceMap = glyphTypeface.AdvanceWidths;

            double maxWidth = 0;
            string[] lines = TextUtils.GetLines(text);
            foreach (var line in lines)
            {
                double lineW = 0;
                foreach (char c in line)
                {
                    if (glyphMap.TryGetValue(c, out ushort glyph))
                    {
                        lineW += advanceMap[glyph] * fontSize;
                    }
                }
                if (lineW > maxWidth)
                    maxWidth = lineW;
            }

            return (int)Math.Ceiling(maxWidth);
        }
    }
}
