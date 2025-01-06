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
        public static void DrawText(this DrawingContext context, string text, Rect bounds, Style style, double pixelPerDip, bool characterEllipses = false)
        {
            context.DrawDrawing(new GlyphRunDrawing(style.Foreground, CreateGlyphRun(text, style, bounds, pixelPerDip, characterEllipses)));
        }

        private static GlyphRun CreateGlyphRun(string text, Style style, Rect bounds, double pixelPerDip, bool characterEllipses)
        {
            ushort[] glyphIndexes = new ushort[text.Length];
            double[] advanceWidths = new double[text.Length];
            double textWidth = 0;

            for (int n = 0; n < text.Length; n++)
            {
                char c = text[n];

                if(characterEllipses && textWidth > bounds.Width - 20)
                {
                    c = '.';
                }

                ushort glyphIndex = style.GlyphTypeface.CharacterToGlyphMap[c];
                glyphIndexes[n] = glyphIndex;
                advanceWidths[n] = style.GlyphTypeface.AdvanceWidths[glyphIndex] * 14;
                textWidth += advanceWidths[n];
            }

            var renderPosition = ComputeTextAlignment(bounds.BottomLeft, bounds, textWidth, style.FontSize, style.HorizontalAlignment, style.VerticalAlignment);
            renderPosition.Offset(0, -pixelPerDip);
            renderPosition.Offset(pixelPerDip, 0);

            return new GlyphRun(style.GlyphTypeface, 0, false, style.FontSize, (float)pixelPerDip, 
                glyphIndexes, renderPosition, advanceWidths, null, null, null,
                                              null, null, null);
        }

        private static Point ComputeTextAlignment(Point defaultRenderPosition, Rect bounds, double textWidth, double textHeight, AlphaXHorizontalAlignment hAlign, AlphaXVerticalAlignment vAlign)
        {
            switch (hAlign)
            {
                case AlphaXHorizontalAlignment.Center:
                    defaultRenderPosition.Offset(bounds.Width / 2 - textWidth / 2, -5);
                    if(defaultRenderPosition.X < bounds.X)
                        defaultRenderPosition.X = bounds.X;
                    break;

                case AlphaXHorizontalAlignment.Left:
                    defaultRenderPosition.Offset(5, -5);
                    break;

                case AlphaXHorizontalAlignment.Right:
                    defaultRenderPosition.Offset(bounds.Width - textWidth - 5, -5);
                    break;
            }

            switch (vAlign)
            {
                case AlphaXVerticalAlignment.Center:
                    defaultRenderPosition.Offset(0, -(bounds.Height / 2 - textHeight / 2));
                    if (defaultRenderPosition.Y < bounds.Y)
                        defaultRenderPosition.Y = bounds.Y;
                    break;

                case AlphaXVerticalAlignment.Top:
                    defaultRenderPosition.Y = bounds.Top;
                    defaultRenderPosition.Offset(0, textHeight + 5);
                    break;
            }

            return defaultRenderPosition;
        }

        public static int ComputeTextWidth(string text, double fontSize, GlyphTypeface glyphTypeface)
        {
            ushort[] glyphIndexes = new ushort[text.Length];
            double[] advanceWidths = new double[text.Length];
            double textWidth = 0;
            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
                glyphIndexes[n] = glyphIndex;
                advanceWidths[n] = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;
                textWidth += advanceWidths[n];
            }
            return (int)textWidth;
        }
    }
}
