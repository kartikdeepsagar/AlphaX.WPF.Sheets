using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Utils;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal static class TextRenderingExtensions
    {
        private const double TextPadding = 5;

        public static void DrawText(
            this DrawingContext context,
            string text,
            Rect bounds,
            WPFStyle style,
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

            double availableWidth = bounds.Width - 2 * TextPadding;
            if (availableWidth <= 0)
                return;

            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                style.Typeface,
                style.FontSize,
                style.Foreground,
                pixelsPerDip);

            formattedText.MaxTextWidth = availableWidth;

            if (!allowMultiLineText)
                formattedText.MaxLineCount = 1;

            if (characterEllipses)
                formattedText.Trimming = TextTrimming.CharacterEllipsis;

            switch (style.HorizontalAlignment)
            {
                case DevBrewLabs.Spreadsheet.HorizontalAlignment.Center:
                    formattedText.TextAlignment = TextAlignment.Center;
                    break;
                case DevBrewLabs.Spreadsheet.HorizontalAlignment.Right:
                    formattedText.TextAlignment = TextAlignment.Right;
                    break;
                default: // Left or Auto
                    formattedText.TextAlignment = TextAlignment.Left;
                    break;
            }

            double textHeight = formattedText.Height;
            double y;
            switch (style.VerticalAlignment)
            {
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Top:
                    y = bounds.Top + TextPadding;
                    break;
                case DevBrewLabs.Spreadsheet.VerticalAlignment.Center:
                    y = bounds.Top + (bounds.Height - textHeight) / 2;
                    if (y < bounds.Top)
                        y = bounds.Top;
                    break;
                default: // Bottom
                    y = bounds.Bottom - TextPadding - textHeight;
                    if (y < bounds.Top)
                        y = bounds.Top;
                    break;
            }

            double x = bounds.Left + TextPadding;
            context.DrawText(formattedText, new Point(x, y));
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
