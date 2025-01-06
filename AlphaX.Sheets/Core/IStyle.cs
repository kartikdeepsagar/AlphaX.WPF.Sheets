using AlphaX.Sheets.Drawing;
using System;

namespace AlphaX.Sheets
{
    public interface IStyle : IDisposable, ICloneable<IStyle>
    {
        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        Color ForeColor { get; set; }
        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        Color BackColor { get; set; }
        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        double FontSize { get; set; }
        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        FontFamily FontFamily { get; set; }
        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        FontWeight FontWeight { get; set; }
        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        FontStyle FontStyle { get; set; }
        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        Thickness Padding { get; set; }
        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        AlphaXVerticalAlignment VerticalAlignment { get; set; }
        /// <summary>
        /// Gets or sets the horizontal aligment.
        /// </summary>
        AlphaXHorizontalAlignment HorizontalAlignment { get; set; }
    }
}
