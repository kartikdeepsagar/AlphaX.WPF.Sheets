using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal static class TextRenderingExtensions
    {
        private const double TextPadding = 5;

        // Fast path handles plain Latin/Latin-Extended text with no glyph
        // fallback needed. Anything above this codepoint (Cyrillic, CJK,
        // Arabic/Hebrew, emoji, combining marks, etc.) needs real shaping
        // and falls back to FormattedText.
        private const char FastPathMaxChar = '\u02FF';

        public static void DrawText(
            this DrawingContext context,
            string text,
            Rect bounds,
            WPFStyle style,
            double pixelsPerDip,
            bool characterEllipses = false,
            bool allowMultiLineText = true,
            double zoomFactor = 1.0)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!allowMultiLineText)
            {
                text = TextUtils.NormalizeToSingleLine(text);
            }

            double textPadding = TextPadding * zoomFactor;
            double availableWidth = bounds.Width - 2 * textPadding;
            if (availableWidth <= 0)
                return;

            // The renderers (e.g., CellsRenderer) already pre-scale style.FontSize by zoom.
            // Do not double-scale it here.
            double scaledFontSize = style.FontSize;

            // Fast path: the vast majority of grid cells are plain,
            // single-line, simple-script text. Skip FormattedText entirely.
            if (TryDrawGlyphRun(context, text, bounds, style, pixelsPerDip, availableWidth, zoomFactor, scaledFontSize, characterEllipses))
                return;

            // ---- Slow path: handles wrapping / complex scripts / fallback ----
            // We force TextFormattingMode.Display to ensure perfectly sharp text
            // across all zoom levels, eliminating subpixel anti-aliasing blur.
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                style.Typeface,
                scaledFontSize,
                style.Foreground,
                null,
                TextFormattingMode.Display,
                pixelsPerDip);

            formattedText.MaxTextWidth = availableWidth;

            if (!allowMultiLineText)
                formattedText.MaxLineCount = 1;

            if (characterEllipses)
                formattedText.Trimming = TextTrimming.None;
            else
                formattedText.Trimming = TextTrimming.CharacterEllipsis;

            switch (style.HorizontalAlignment)
            {
                case DevBrewLabs.Spreadsheet.HorizontalAlignment.Center:
                    formattedText.TextAlignment = TextAlignment.Center;
                    break;
                case DevBrewLabs.Spreadsheet.HorizontalAlignment.Right:
                    formattedText.TextAlignment = TextAlignment.Right;
                    break;
                default:
                    formattedText.TextAlignment = TextAlignment.Left;
                    break;
            }

            double textHeight = formattedText.Height;
            double y;
            switch (style.VerticalAlignment)
            {
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Top:
                    y = bounds.Top + textPadding;
                    break;
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Center:
                    y = bounds.Top + (bounds.Height - textHeight) / 2;
                    if (y < bounds.Top)
                        y = bounds.Top;
                    break;
                default:
                    y = bounds.Bottom - textPadding - textHeight;
                    if (y < bounds.Top)
                        y = bounds.Top;
                    break;
            }

            double x = bounds.Left + textPadding;

            // Crucial: Align to physical pixel boundaries to prevent blur
            x = RoundToPixel(x, pixelsPerDip);
            y = RoundToPixel(y, pixelsPerDip);
            
            context.DrawText(formattedText, new Point(x, y));
        }

        private static bool TryDrawGlyphRun(
            DrawingContext context,
            string text,
            Rect bounds,
            WPFStyle style,
            double pixelsPerDip,
            double availableWidth,
            double zoomFactor,
            double scaledFontSize,
            bool characterEllipses)
        {
            int len = text.Length;
            if (len == 0)
                return true;

            var glyphTypeface = style.GlyphTypeface;
            if (glyphTypeface == null)
                return false;

            var characterToGlyphMap = glyphTypeface.CharacterToGlyphMap;
            var advanceWidthsMap = glyphTypeface.AdvanceWidths;

            // 1. Measuring Phase: Check if fast path is viable and determine total width
            double exactTotalWidth = 0;
            int fitCount = len;
            bool isTruncated = false;
            ushort ellipsisGlyph = 0;
            double exactEllipsisWidth = 0;

            for (int i = 0; i < len; i++)
            {
                char c = text[i];
                if (c == '\n' || c > FastPathMaxChar || (char.IsControl(c) && c != '\t'))
                    return false; // multi-line, complex script, or control chars

                if (!characterToGlyphMap.TryGetValue(c, out ushort glyph) || glyph == 0)
                    return false; // missing glyph

                double exactAdvance = advanceWidthsMap[glyph] * scaledFontSize;
                double snappedNextX = Math.Round((exactTotalWidth + exactAdvance) * pixelsPerDip) / pixelsPerDip;
                
                if (snappedNextX > availableWidth)
                {
                    fitCount = i;
                    isTruncated = true;
                    break;
                }

                exactTotalWidth += exactAdvance;
            }

            // 2. Truncation Phase (if needed)
            if (isTruncated && !characterEllipses)
            {
                if (!characterToGlyphMap.TryGetValue('\u2026', out ellipsisGlyph) || ellipsisGlyph == 0)
                    return false; // fallback to FormattedText if ellipsis is missing

                exactEllipsisWidth = advanceWidthsMap[ellipsisGlyph] * scaledFontSize;

                // Backtrack to fit the ellipsis
                while (fitCount > 0)
                {
                    double snappedNextX = Math.Round((exactTotalWidth + exactEllipsisWidth) * pixelsPerDip) / pixelsPerDip;
                    
                    if (snappedNextX <= availableWidth)
                        break;
                        
                    fitCount--;
                    char c = text[fitCount];
                    if (characterToGlyphMap.TryGetValue(c, out ushort glyph))
                    {
                        exactTotalWidth -= advanceWidthsMap[glyph] * scaledFontSize;
                    }
                }

                if (fitCount == 0 && Math.Round(exactEllipsisWidth * pixelsPerDip) / pixelsPerDip > availableWidth)
                    return false; // doesn't even fit an ellipsis
            }

            int finalGlyphCount = isTruncated && !characterEllipses ? fitCount + 1 : fitCount;

            if (finalGlyphCount <= 0)
                return true; // Nothing to draw

            // 3. Allocation & Population Phase (with pixel snapping for sharpness)
            var glyphIndices = new ushort[finalGlyphCount];
            var advanceWidths = new double[finalGlyphCount];
            
            double runningExactX = 0;

            for (int i = 0; i < fitCount; i++)
            {
                characterToGlyphMap.TryGetValue(text[i], out ushort glyph);
                glyphIndices[i] = glyph;
                
                double exactAdvance = advanceWidthsMap[glyph] * scaledFontSize;
                double exactNextX = runningExactX + exactAdvance;
                
                // Snap each character to the nearest physical pixel to completely 
                // eliminate sub-pixel rendering blur (emulates TextFormattingMode.Display)
                double snappedCurrentX = Math.Round(runningExactX * pixelsPerDip) / pixelsPerDip;
                double snappedNextX = Math.Round(exactNextX * pixelsPerDip) / pixelsPerDip;
                
                advanceWidths[i] = snappedNextX - snappedCurrentX;
                runningExactX = exactNextX;
            }

            if (isTruncated && !characterEllipses)
            {
                glyphIndices[fitCount] = ellipsisGlyph;
                
                double exactNextX = runningExactX + exactEllipsisWidth;
                double snappedCurrentX = Math.Round(runningExactX * pixelsPerDip) / pixelsPerDip;
                double snappedNextX = Math.Round(exactNextX * pixelsPerDip) / pixelsPerDip;
                
                advanceWidths[fitCount] = snappedNextX - snappedCurrentX;
                runningExactX = exactNextX;
            }

            double totalSnappedWidth = Math.Round(runningExactX * pixelsPerDip) / pixelsPerDip;

            // 4. Alignment & Placement Phase
            double textPadding = TextPadding * zoomFactor;
            double x;
            switch (style.HorizontalAlignment)
            {
                case DevBrewLabs.Spreadsheet.HorizontalAlignment.Center:
                    x = bounds.Left + (bounds.Width - totalSnappedWidth) / 2;
                    break;
                case DevBrewLabs.Spreadsheet.HorizontalAlignment.Right:
                    x = bounds.Right - textPadding - totalSnappedWidth;
                    break;
                default:
                    x = bounds.Left + textPadding;
                    break;
            }
            if (x < bounds.Left + textPadding)
                x = bounds.Left + textPadding;

            double ascent = glyphTypeface.Baseline * scaledFontSize;
            double lineHeight = glyphTypeface.Height * scaledFontSize;
            double y;
            switch (style.VerticalAlignment)
            {
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Top:
                    y = bounds.Top + textPadding + ascent;
                    break;
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Center:
                    y = bounds.Top + (bounds.Height - lineHeight) / 2 + ascent;
                    if (y - ascent < bounds.Top)
                        y = bounds.Top + ascent;
                    break;
                default:
                    y = bounds.Bottom - textPadding - lineHeight + ascent;
                    if (y - ascent < bounds.Top)
                        y = bounds.Top + ascent;
                    break;
            }

            // Snap the baseline to the physical pixel grid
            x = RoundToPixel(x, pixelsPerDip);
            y = RoundToPixel(y, pixelsPerDip);

            // 5. Drawing Phase
            var glyphRun = new GlyphRun(
                glyphTypeface,
                0,                      // bidiLevel
                false,                  // sideways
                scaledFontSize,         // renderingEmSize
                (float)pixelsPerDip,
                glyphIndices,
                new Point(x, y),         // baselineOrigin
                advanceWidths,
                null,                    // glyphOffsets
                null,                    // characters
                null,                    // deviceFontName
                null,                    // clusterMap
                null,                    // caretStops
                null);                   // language

            context.DrawGlyphRun(style.Foreground, glyphRun);
            return true;
        }

        private static double RoundToPixel(double value, double pixelsPerDip)
        {
            return Math.Round(value * pixelsPerDip) / pixelsPerDip;
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
            double currentLineWidth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '\n')
                {
                    if (currentLineWidth > maxWidth)
                        maxWidth = currentLineWidth;
                    currentLineWidth = 0;
                    continue;
                }
                
                // Ignore carriage return
                if (c == '\r')
                    continue;

                if (glyphMap.TryGetValue(c, out ushort glyph))
                {
                    currentLineWidth += advanceMap[glyph] * fontSize;
                }
            }

            // Handle the last line
            if (currentLineWidth > maxWidth)
                maxWidth = currentLineWidth;

            return (int)Math.Ceiling(maxWidth);
        }
    }
}