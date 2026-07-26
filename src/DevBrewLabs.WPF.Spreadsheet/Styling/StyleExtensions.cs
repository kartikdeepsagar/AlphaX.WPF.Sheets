using DevBrewLabs.Spreadsheet;
using System.Runtime.CompilerServices;

namespace DevBrewLabs.WPF.Spreadsheet
{
    public static class StyleExtensions
    {
        /// <summary>
        /// Gets the WPF Style instance from an IStyle reference with zero-overhead inlining.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WPFStyle GetWpfStyle(this IStyle style)
        {
            return style as WPFStyle;
        }
    }
}
