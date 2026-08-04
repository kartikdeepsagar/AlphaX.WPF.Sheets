using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Styling
{
    internal class WpfFontResources
    {
        public Typeface Typeface { get; }
        public GlyphTypeface GlyphTypeface { get; }
        public GlyphMetrics GlyphMetrics { get; }

        public WpfFontResources(Typeface typeface, GlyphTypeface glyphTypeface, GlyphMetrics metrics)
        {
            Typeface = typeface;
            GlyphTypeface = glyphTypeface;
            GlyphMetrics = metrics;
        }
    }
}
