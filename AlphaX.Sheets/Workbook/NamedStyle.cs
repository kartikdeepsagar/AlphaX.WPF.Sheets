using AlphaX.Sheets.Drawing;
using System.ComponentModel;

namespace AlphaX.Sheets
{
    public class NamedStyle : IStyle, INotifyPropertyChanged
    {
        private Color? _foreColor;
        private Color? _backColor;
        private double? _fontSize;
        private FontFamily _fontFamily;
        private FontWeight? _fontWeight;
        private FontStyle? _fontStyle;
        private Thickness? _padding;
        private AlphaXHorizontalAlignment? _hAligment;
        private AlphaXVerticalAlignment? _vAligment;

        public event PropertyChangedEventHandler PropertyChanged;

        public Color ForeColor
        {
            get
            {
                return _foreColor.Value;
            }
            set
            {
                _foreColor = value;
                OnPropertyChanged(nameof(ForeColor));

            }
        }

        public Color BackColor
        {
            get
            {
                return _backColor.Value;
            }
            set
            {
                _backColor = value;
                OnPropertyChanged(nameof(BackColor));

            }
        }

        public double FontSize
        {
            get
            {
                return _fontSize.Value;
            }
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));

            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                _fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));

            }
        }

        public FontWeight FontWeight
        {
            get
            {
                return _fontWeight.Value;
            }
            set
            {
                _fontWeight = value;
                OnPropertyChanged(nameof(FontWeight));
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle.Value;
            }
            set
            {
                _fontStyle = value;
                OnPropertyChanged(nameof(FontStyle));
            }
        }

        public Thickness Padding
        {
            get
            {
                return _padding.Value;
            }
            set
            {
                _padding = value;
                OnPropertyChanged(nameof(Padding));
            }
        }

        public AlphaXHorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _hAligment.Value;
            }
            set
            {
                _hAligment = value;
                OnPropertyChanged(nameof(HorizontalAlignment));
            }
        }

        public AlphaXVerticalAlignment VerticalAlignment
        {
            get
            {
                return _vAligment.Value;
            }
            set
            {
                _vAligment = value;
                OnPropertyChanged(nameof(VerticalAlignment));
            }
        }

        public NamedStyle()
        {
            ForeColor = Color.Black;
            BackColor = Color.Transparent;
            FontSize = 14;
            FontFamily = new FontFamily("Calibri");
            FontWeight = FontWeight.Regular;
            FontStyle = FontStyle.Normal;
            Padding = new Thickness(5, 5);
            HorizontalAlignment = AlphaXHorizontalAlignment.Auto;
            VerticalAlignment = AlphaXVerticalAlignment.Auto;
        }

        public void Dispose()
        {
            _foreColor = null;
            _backColor = null;
            _fontSize = null;
            _fontFamily = null;
            _fontWeight = null;
            _fontStyle = null;
            _padding = null;
            _hAligment = null;
            _vAligment = null;
        }

        public virtual IStyle Clone()
        {
            return new NamedStyle()
            {
                BackColor = _backColor.Value,
                FontFamily = _fontFamily,
                FontSize = _fontSize.Value,
                FontWeight = _fontWeight.Value,
                FontStyle = _fontStyle.Value,
                ForeColor = _foreColor.Value,
                HorizontalAlignment = _hAligment.Value,
                VerticalAlignment = _vAligment.Value,
                Padding = _padding.Value,
            };
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}