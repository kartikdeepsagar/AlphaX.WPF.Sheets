using System;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal static class PixelSnapper
    {
        public static double Snap(double value, double pixelsPerDip)
        {
            return Math.Round(value * pixelsPerDip) / pixelsPerDip;
        }
    }
}
