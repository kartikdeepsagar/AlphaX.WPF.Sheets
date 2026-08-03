using DevBrewLabs.Spreadsheet;
using DevBrewLabs.WPF.Spreadsheet.Rendering.Text;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet
{
    public class WPFStyle : DevBrewLabs.Spreadsheet.Style
    {
        private Typeface _typeFace;
        private GlyphTypeface _glyphTypeface;
        private GlyphMetrics _glyphMetrics;

        internal Brush Background { get; private set; }
        internal Brush Foreground { get; private set; }
        internal FontFamily WpfFontFamily { get; private set; }
        internal FontWeight WpfFontWeight { get; private set; }
        internal FontStyle WpfFontStyle { get; private set; }
        internal Thickness WpfPadding { get; private set; }

        internal GlyphTypeface GlyphTypeface
        {
            get
            {
                if (_glyphTypeface == null)
                    CreateTypeFace();

                return _glyphTypeface;
            }
        }

        internal Rendering.Text.GlyphMetrics GlyphMetrics
        {
            get
            {
                if (_glyphMetrics == null)
                    CreateTypeFace();

                return _glyphMetrics;
            }
        }

        internal Typeface Typeface
        {
            get
            {
                if (_typeFace == null)
                    CreateTypeFace(false);

                return _typeFace;
            }
        }

        public void SetBackground(DevBrewLabs.Spreadsheet.Drawing.Color color)
        {
            Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            Background.Freeze();
        }

        public void SetForeground(DevBrewLabs.Spreadsheet.Drawing.Color color)
        {
            Foreground = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            Foreground.Freeze();
        }

        public void SetFontFamily(DevBrewLabs.Spreadsheet.Drawing.FontFamily fontFamily)
        {
            WpfFontFamily = new FontFamily(fontFamily.FamilyName);
            CreateTypeFace();
        }

        public void SetPadding(DevBrewLabs.Spreadsheet.Drawing.Thickness thickness)
        {
            WpfPadding = new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        public void SetFontWeight(DevBrewLabs.Spreadsheet.Drawing.FontWeight fontWeight)
        {
            switch(fontWeight)
            {
                case DevBrewLabs.Spreadsheet.Drawing.FontWeight.Bold:
                    WpfFontWeight = FontWeights.Bold;
                    break;

                case DevBrewLabs.Spreadsheet.Drawing.FontWeight.Regular:
                    WpfFontWeight = FontWeights.Regular;
                    break;

                case DevBrewLabs.Spreadsheet.Drawing.FontWeight.Normal:
                    WpfFontWeight = FontWeights.Normal;
                    break;
            }
            CreateTypeFace();
        }

        public void SetFontStyle(DevBrewLabs.Spreadsheet.Drawing.FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case DevBrewLabs.Spreadsheet.Drawing.FontStyle.Italic:
                    WpfFontStyle = FontStyles.Italic;
                    break;

                case DevBrewLabs.Spreadsheet.Drawing.FontStyle.Oblique:
                    WpfFontStyle = FontStyles.Oblique;
                    break;

                case DevBrewLabs.Spreadsheet.Drawing.FontStyle.Normal:
                    WpfFontStyle = FontStyles.Normal;
                    break;
            }
            CreateTypeFace();
        }

        private void CreateTypeFace(bool createGlyph = true)
        {
            _typeFace = new Typeface(WpfFontFamily, WpfFontStyle, WpfFontWeight, FontStretches.Normal, new FontFamily("Arial"));
            
            if(createGlyph)
            {
                _typeFace.TryGetGlyphTypeface(out _glyphTypeface);
                if (_glyphTypeface != null)
                    _glyphMetrics = new Rendering.Text.GlyphMetrics(_glyphTypeface);
            }
        }

        public override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch(propertyName)
            {
                case "ForeColor":
                    SetForeground(ForeColor);
                    break;

                case "BackColor":
                    SetBackground(BackColor);
                    break;

                case "FontFamily":
                    SetFontFamily(FontFamily);
                    break;

                case "FontWeight":
                    SetFontWeight(FontWeight);
                    break;

                case "FontStyle":
                    SetFontStyle(FontStyle);
                    break;

                case "Padding":
                    SetPadding(Padding);
                    break;
            }
        }

        public override IStyle Clone()
        {
            return new WPFStyle()
            {
                _glyphTypeface = this._glyphTypeface,
                _glyphMetrics = this._glyphMetrics,
                BackColor = base.BackColor,
                FontFamily = base.FontFamily,
                FontWeight = base.FontWeight,
                FontStyle = base.FontStyle,
                Padding = base.Padding,
                FontSize = base.FontSize,
                ForeColor = base.ForeColor,
                VerticalAlignment = base.VerticalAlignment,
                HorizontalAlignment = base.HorizontalAlignment,
            };
        }
    }
}
