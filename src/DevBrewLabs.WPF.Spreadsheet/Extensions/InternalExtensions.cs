using DevBrewLabs.Spreadsheet;
using DevBrewLabs.Spreadsheet.Formatters;
using DevBrewLabs.WPF.Spreadsheet.UI.Interaction;
using System.Windows;

namespace DevBrewLabs.WPF.Spreadsheet
{
    internal static class InternalExtensions
    {
        public static T As<T>(this object obj)
        {
            return (T)obj;
        }

        internal static bool ContainsOrIntersectsWith(this Rect source, Rect rect)
        {
            return source.Contains(rect) || source.IntersectsWith(rect);
        }



        internal static void EnsureFree(this InteractionLayer layer)
        {
            if (!layer.IsAttached)
                return;

            layer.DetachFromRegion();
            layer.ReleaseMouseCapture();
            layer.InvalidateVisual();
        }
    }
}
