using AlphaX.Sheets;
using System;
using System.Diagnostics;
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
            bool characterEllipses = false)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var glyphRun = CreateGlyphRun(text, style, bounds, pixelsPerDip, characterEllipses);

            if (glyphRun != null)
            {
                context.DrawDrawing(new GlyphRunDrawing(style.Foreground, glyphRun));
            }
        }

        private static GlyphRun CreateGlyphRun(
            string text,
            Style style,
            Rect bounds,
            double pixelsPerDip,
            bool characterEllipses)
        {
            var glyphTypeface = style.GlyphTypeface;
            var glyphMap = glyphTypeface.CharacterToGlyphMap;
            var advanceMap = glyphTypeface.AdvanceWidths;

            double fontSize = style.FontSize;

            ushort[] glyphIndexes = new ushort[text.Length];
            double[] advanceWidths = new double[text.Length];

            int glyphCount = 0;
            double textWidth = 0;

            ushort dotGlyph = glyphMap['.'];
            double dotWidth = advanceMap[dotGlyph] * fontSize;
            double ellipsisWidth = dotWidth * 3;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                ushort glyph = glyphMap[c];
                double advance = advanceMap[glyph] * fontSize;

                if (characterEllipses &&
                    i < text.Length - 1 &&
                    textWidth + advance > bounds.Width - ellipsisWidth)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        glyphIndexes[glyphCount] = dotGlyph;
                        advanceWidths[glyphCount] = dotWidth;
                        glyphCount++;
                    }

                    textWidth += ellipsisWidth;
                    break;
                }

                glyphIndexes[glyphCount] = glyph;
                advanceWidths[glyphCount] = advance;
                glyphCount++;

                textWidth += advance;
            }

            if (glyphCount != glyphIndexes.Length)
            {
                Array.Resize(ref glyphIndexes, glyphCount);
                Array.Resize(ref advanceWidths, glyphCount);
            }

            Point renderPosition = ComputeTextAlignment(
                bounds.BottomLeft,
                bounds,
                textWidth,
                fontSize,
                style.HorizontalAlignment,
                style.VerticalAlignment);

            renderPosition.Offset(pixelsPerDip, -pixelsPerDip);

            return new GlyphRun(
                glyphTypeface,
                0,
                false,
                fontSize,
                (float)pixelsPerDip,
                glyphIndexes,
                renderPosition,
                advanceWidths,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private static Point ComputeTextAlignment(
            Point renderPosition,
            Rect bounds,
            double textWidth,
            double textHeight,
            AlphaXHorizontalAlignment hAlign,
            AlphaXVerticalAlignment vAlign)
        {
            switch (hAlign)
            {
                case AlphaXHorizontalAlignment.Center:
                    renderPosition.Offset((bounds.Width - textWidth) / 2, -TextPadding);

                    if (renderPosition.X < bounds.X)
                        renderPosition.X = bounds.X;

                    break;

                case AlphaXHorizontalAlignment.Left:
                    renderPosition.Offset(TextPadding, -TextPadding);
                    break;

                case AlphaXHorizontalAlignment.Right:
                    renderPosition.Offset(bounds.Width - textWidth - TextPadding, -TextPadding);
                    break;
            }

            switch (vAlign)
            {
                case AlphaXVerticalAlignment.Center:
                    renderPosition.Offset(0, -(bounds.Height - textHeight) / 2);

                    if (renderPosition.Y < bounds.Y)
                        renderPosition.Y = bounds.Y;

                    break;

                case AlphaXVerticalAlignment.Top:
                    renderPosition.Y = bounds.Top + textHeight + TextPadding;
                    break;
            }

            return renderPosition;
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

            double width = 0;

            foreach (char c in text)
            {
                ushort glyph = glyphMap[c];
                width += advanceMap[glyph] * fontSize;
            }

            return (int)Math.Ceiling(width);
        }
    }
}
