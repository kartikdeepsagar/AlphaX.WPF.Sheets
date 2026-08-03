namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal readonly struct RenderContext
    {
        public double Zoom { get; }
        public double PixelsPerDip { get; }
        public double TextPadding { get; }
        public bool SnapToPixels { get; }

        public RenderContext(double zoom, double pixelsPerDip, double textPadding, bool snapToPixels)
        {
            Zoom = zoom;
            PixelsPerDip = pixelsPerDip;
            TextPadding = textPadding;
            SnapToPixels = snapToPixels;
        }
    }
}
