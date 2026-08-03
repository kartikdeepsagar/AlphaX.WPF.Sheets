using DevBrewLabs.Spreadsheet.Formatters;

namespace DevBrewLabs.Spreadsheet
{
    public static class WorkBookExtensions
    {
        /// <summary>
        /// Gets the style according to the priority.
        /// </summary>
        public static IStyle PickStyle(this IWorkBook workBook, IRange cell, IColumn column, IRow row, SheetRegion region)
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
        public static IFormatter PickFormatter(this IWorkSheet sheet, IRange cell, IColumn column, IRow row)
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
    }
}
