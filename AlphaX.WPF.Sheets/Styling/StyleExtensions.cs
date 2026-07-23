using AlphaX.Sheets;
using System.Runtime.CompilerServices;

namespace AlphaX.WPF.Sheets
{
    public static class StyleExtensions
    {
        /// <summary>
        /// Gets the WPF AlphaXStyle instance from an IStyle reference with zero-overhead inlining.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AlphaXStyle GetWpfStyle(this IStyle style)
        {
            return style as AlphaXStyle;
        }
    }
}
