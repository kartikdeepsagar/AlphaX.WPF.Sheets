using AlphaX.Sheets;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets
{
    public class Style : NamedStyle
    {
        private Typeface _typeFace;
        private GlyphTypeface _glyphTypeface;

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

        internal Typeface Typeface
        {
            get
            {
                if (_typeFace == null)
                    CreateTypeFace(false);

                return _typeFace;
            }
        }

        public void SetBackground(AlphaX.Sheets.Drawing.Color color)
        {
            Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public void SetForeground(AlphaX.Sheets.Drawing.Color color)
        {
            Foreground = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public void SetFontFamily(AlphaX.Sheets.Drawing.FontFamily fontFamily)
        {
            WpfFontFamily = new FontFamily(fontFamily.FamilyName);
            CreateTypeFace();
        }

        public void SetPadding(AlphaX.Sheets.Drawing.Thickness thickness)
        {
            WpfPadding = new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        public void SetFontWeight(AlphaX.Sheets.Drawing.FontWeight fontWeight)
        {
            switch(fontWeight)
            {
                case AlphaX.Sheets.Drawing.FontWeight.Bold:
                    WpfFontWeight = FontWeights.Bold;
                    break;

                case AlphaX.Sheets.Drawing.FontWeight.Regular:
                    WpfFontWeight = FontWeights.Regular;
                    break;

                case AlphaX.Sheets.Drawing.FontWeight.Normal:
                    WpfFontWeight = FontWeights.Normal;
                    break;
            }
            CreateTypeFace();
        }

        public void SetFontStyle(AlphaX.Sheets.Drawing.FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case AlphaX.Sheets.Drawing.FontStyle.Italic:
                    WpfFontStyle = FontStyles.Italic;
                    break;

                case AlphaX.Sheets.Drawing.FontStyle.Oblique:
                    WpfFontStyle = FontStyles.Oblique;
                    break;

                case AlphaX.Sheets.Drawing.FontStyle.Normal:
                    WpfFontStyle = FontStyles.Normal;
                    break;
            }
            CreateTypeFace();
        }

        private void CreateTypeFace(bool createGlyph = true)
        {
            _typeFace = new Typeface(WpfFontFamily, WpfFontStyle, WpfFontWeight, FontStretches.Normal, new FontFamily("Arial"));
            
            if(createGlyph)
                _typeFace.TryGetGlyphTypeface(out _glyphTypeface);
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
            return new Style()
            {
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
