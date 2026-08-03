namespace DevBrewLabs.WPF.Spreadsheet.Rendering.Text
{
    internal class TextLayout
    {
        public ushort[] GlyphIndices { get; }
        public double[] AdvanceWidths { get; }
        public double Width { get; }
        public double Height { get; }
        public int GlyphCount { get; }
        public bool IsTruncated { get; }

        public TextLayout(ushort[] glyphIndices, double[] advanceWidths, double width, double height, int glyphCount, bool isTruncated)
        {
            GlyphIndices = glyphIndices;
            AdvanceWidths = advanceWidths;
            Width = width;
            Height = height;
            GlyphCount = glyphCount;
            IsTruncated = isTruncated;
        }
    }
}
