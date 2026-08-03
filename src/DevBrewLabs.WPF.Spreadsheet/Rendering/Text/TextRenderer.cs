using DevBrewLabs.Spreadsheet.Utils;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal static class TextRenderer
    {
        public static void DrawText(
            DrawingContext drawingContext,
            string text,
            Rect bounds,
            WPFStyle style,
            RenderContext renderContext,
            bool characterEllipses = false,
            bool allowMultiLineText = true)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!allowMultiLineText)
            {
                text = TextUtils.NormalizeToSingleLine(text);
            }

            double availableWidth = bounds.Width - (2 * renderContext.TextPadding);
            if (availableWidth <= 0)
                return;

            if (!CharacterAnalyzer.IsSupported(text))
            {
                // Fallback behavior for unsupported scripts (e.g. Emoji, Arabic).
                // We convert it to a string of '?' if we have a replacement glyph.
                if (style.GlyphMetrics.ReplacementGlyph != 0)
                {
                    text = new string('?', text.Length);
                }
                else
                {
                    // Skip rendering
                    return;
                }
            }

            double scaledFontSize = style.FontSize * renderContext.Zoom;

            string[] lines = allowMultiLineText && text.IndexOf('\n') >= 0 
                ? text.Split('\n') 
                : new[] { text };

            // We pre-calculate total height to support Vertical alignment
            double totalHeight = 0;
            TextLayout[] layouts = new TextLayout[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                // Remove \r if present
                string line = lines[i].TrimEnd('\r');
                layouts[i] = TextLayoutCache.GetOrCreate(line, availableWidth, scaledFontSize, style, renderContext, characterEllipses);
                totalHeight += layouts[i].Height;
            }

            double startY;
            switch (style.VerticalAlignment)
            {
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Top:
                    startY = bounds.Top + renderContext.TextPadding;
                    break;
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Center:
                    startY = bounds.Top + (bounds.Height - totalHeight) / 2;
                    if (startY < bounds.Top)
                        startY = bounds.Top;
                    break;
                default: // Bottom
                    startY = bounds.Bottom - renderContext.TextPadding - totalHeight;
                    if (startY < bounds.Top)
                        startY = bounds.Top;
                    break;
            }

            startY = PixelSnapper.Snap(startY, renderContext.PixelsPerDip);
            double currentY = startY;
            double ascent = style.GlyphMetrics.Baseline * scaledFontSize;

            for (int i = 0; i < layouts.Length; i++)
            {
                var layout = layouts[i];
                if (layout.GlyphCount > 0)
                {
                    double x;
                    switch (style.HorizontalAlignment)
                    {
                        case DevBrewLabs.Spreadsheet.HorizontalAlignment.Center:
                            x = bounds.Left + (bounds.Width - layout.Width) / 2;
                            break;
                        case DevBrewLabs.Spreadsheet.HorizontalAlignment.Right:
                            x = bounds.Right - renderContext.TextPadding - layout.Width;
                            break;
                        default: // Left
                            x = bounds.Left + renderContext.TextPadding;
                            break;
                    }

                    if (x < bounds.Left + renderContext.TextPadding)
                        x = bounds.Left + renderContext.TextPadding;

                    x = PixelSnapper.Snap(x, renderContext.PixelsPerDip);

                    Point baselineOrigin = new Point(x, PixelSnapper.Snap(currentY + ascent, renderContext.PixelsPerDip));
                    
                    var glyphRun = GlyphRunFactory.Create(layout, style.GlyphMetrics, scaledFontSize, renderContext, baselineOrigin);
                    
                    if (glyphRun != null)
                    {
                        drawingContext.DrawGlyphRun(style.Foreground, glyphRun);
                    }
                }

                currentY += layout.Height;
            }
        }
    }
}
