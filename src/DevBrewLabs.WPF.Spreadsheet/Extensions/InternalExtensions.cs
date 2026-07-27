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

        /// <summary>
        /// Gets the style according to the priority.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        internal static IStyle PickStyle(this WorkBook workBook, IRange cell, IColumn column, IRow row, SheetRegion region)
        {
            if (cell != null && cell.Style != null)
            {
                return cell.Style;
            }

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

            switch (region)
            {
                case SheetRegion.CornerHeader:
                    return workBook.GetNamedStyle(StyleKeys.DefaultTopLeftStyleKey);

                case SheetRegion.RowHeader:
                    return workBook.GetNamedStyle(StyleKeys.DefaultRowHeaderStyleKey);

                case SheetRegion.ColumnHeader:
                    return workBook.GetNamedStyle(StyleKeys.DefaultColumnHeaderStyleKey);

                default:
                    return workBook.GetNamedStyle(StyleKeys.DefaultSheetStyleKey);
            }
        }

        /// <summary>
        /// Gets the formatter according to the priority.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        internal static IFormatter PickFormatter(this IWorkSheet sheet, IRange cell, IColumn column, IRow row)
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

            return GeneralFormatter.Default;
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
