using AlphaX.Sheets;
using AlphaX.Sheets.Formatters;
using AlphaX.WPF.Sheets.UI.Interaction;
using System.Windows;

namespace AlphaX.WPF.Sheets
{
    internal static class InternalExtensions
    {
        public static IFormatter DefaultFormatter { get; }

        static InternalExtensions()
        {
            DefaultFormatter = new GeneralFormatter();
        }

        public static T As<T>(this object obj)
        {
            return (T)obj;
        }

        internal static bool ContainsOrIntersectsWith(this Rect source, Rect rect)
        {
            return source.Contains(rect) || source.IntersectsWith(rect);
        }

        /// <summary>
        /// Gets the style according to the priority.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        internal static IStyle PickStyle(this WorkBook workBook, ICell cell, IColumn column, IRow row)
        {
            if (cell != null && !string.IsNullOrEmpty(cell.StyleName))
            {
                return workBook.GetNamedStyle(cell.StyleName);
            }

            if (column != null && !string.IsNullOrEmpty(column.StyleName))
            {
                return workBook.GetNamedStyle(column.StyleName);
            }

            if (row != null && !string.IsNullOrEmpty(row.StyleName))
            {
                return workBook.GetNamedStyle(row.StyleName);
            }

            return null;
        }

        /// <summary>
        /// Gets the formatter according to the priority.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        internal static IFormatter PickFormatter(this IWorkSheet sheet, ICell cell, IColumn column, IRow row)
        {
            if (cell != null && cell.Formatter != null)
            {
                return cell.Formatter;
            }

            if (column != null && column.Formatter != null)
            {
                return column.Formatter;
            }

            if (row != null && row.Formatter != null)
            {
                return row.Formatter;
            }

            return DefaultFormatter;
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
