using AlphaX.Sheets.Filtering;
using System;

namespace AlphaX.Sheets
{
    public enum WorkSheetClearMode
    {
        Data,
        Formula,
        Styles,
        All
    }

    public interface IWorkSheet : IDisposable
    {
        /// <summary>
        /// Fires when cell is modified.
        /// </summary>
        event EventHandler<CellChangedEventArgs> CellChanged;
        /// <summary>
        /// Fires when a range is sorted.
        /// </summary>
        event EventHandler<RangeChangedEventArgs> RangeSorted;
        /// <summary>
        /// Fires when row/rows changes.
        /// </summary>
        event EventHandler<RowChangedEventArgs> RowsChanged;
        /// <summary>
        /// Fires when column/columns changes.
        /// </summary>
        event EventHandler<ColumnChangedEventArgs> ColumnsChanged;

        /// <summary>
        /// Gets the parent workbook.
        /// </summary>
        WorkBook WorkBook { get; }
        /// <summary>
        /// Gets or sets name for this sheet.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the row count for this sheet.
        /// </summary>
        int RowCount { get; set; }
        /// <summary>
        /// Gets or sets the column count for this sheet.
        /// </summary>
        int ColumnCount { get; set; }
        /// <summary>
        /// Gets or sets the default row height for this sheet.
        /// </summary>
        int DefaultRowHeight { get; set; }
        /// <summary>
        /// Gets or sets the default column width for this sheet.
        /// </summary>
        int DefaultColumnWidth { get; set; }
        /// <summary>
        /// Gets row collection of this sheet.
        /// </summary>
        Rows Rows { get; }
        /// <summary>
        /// Gets column collection of this sheet.
        /// </summary>
        Columns Columns { get; }
        /// <summary>
        /// Gets the cells of this sheet.
        /// </summary>
        Cells Cells { get; }
        /// <summary>
        /// Gets the sheet top left region.
        /// </summary>
        TopLeft TopLeft { get; }
        /// <summary>
        /// Gets the sheet row headers.
        /// </summary>
        RowHeaders RowHeaders { get; }
        /// <summary>
        /// Gets the sheet column headers.
        /// </summary>
        ColumnHeaders ColumnHeaders { get; }
        /// <summary>
        /// Gets or sets the sheet data source.
        /// </summary>
        object DataSource { get; set; }
        /// <summary>
        /// Gets the filter provider.
        /// </summary>
        FilterProvider FilterProvider { get; }
        /// <summary>
        /// Gets the range data.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        object[,] GetData(CellRange range);
        /// <summary>
        /// Gets the range data.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        object[,] GetData(int row, int column, int rowCount, int columnCount);
        /// <summary>
        /// Reevaluates all formulas for this sheet.
        /// </summary>
        void CalculateAll();
        /// <summary>
        /// Sorts the provided cell range.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="ascending"></param>
        void SortRange(CellRange range, bool ascending);
        /// <summary>
        /// Clears worksheet.
        /// </summary>
        void Clear(WorkSheetClearMode mode);
        bool ContainsRange(int row, int column, int rowCount, int columnCount);
    }
}