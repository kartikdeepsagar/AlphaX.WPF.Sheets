namespace AlphaX.Sheets
{
    /// <summary>
    /// Represents a cell range in worbook
    /// </summary>
    public class CellRange
    {
        /// <summary>
        /// Gets whether this range contains only single cell.
        /// </summary>
        public bool IsSingleCell
        {
            get
            {
                return RowCount == 1 && ColumnCount == 1;
            }
        }
        /// <summary>
        /// Gets the top row in this range.
        /// </summary>
        public int TopRow { get; internal set; }
        /// <summary>
        /// Gets the bottom row in this range.
        /// </summary>
        public int BottomRow
        {
            get
            {
                return RowCount == 0 ? -1 : TopRow + RowCount - 1;
            }
        }
        /// <summary>
        /// Gets the left column in this range.
        /// </summary>
        public int LeftColumn { get; internal set; }
        /// <summary>
        /// Gets the right column in this range.
        /// </summary>
        public int RightColumn
        {
            get
            {
                return ColumnCount == 0 ? -1 : LeftColumn + ColumnCount - 1;
            }
        }
        /// <summary>
        /// Gets the row count of this range.
        /// </summary>
        public int RowCount { get; internal set; }
        /// <summary>
        /// Gets the column count of this range.
        /// </summary>
        public int ColumnCount { get; internal set; }
        /// <summary>
        /// Gets whether this range is valid or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return TopRow > -1 && BottomRow > -1 && RowCount > 0 && ColumnCount > 0
                    && TopRow <= BottomRow && LeftColumn <= RightColumn;
            }
        }
        public CellRange(int row, int col)
        {
            TopRow = row;
            LeftColumn = col;
            RowCount = row < 0 ? 0 : 1;
            ColumnCount = col < 0 ? 0 : 1;
        }
        public CellRange(int row, int col, int rowCount, int columnCount)
        {
            TopRow = row;
            LeftColumn = col;
            RowCount = row < 0 ? 0 : rowCount;
            ColumnCount = col < 0 ? 0 : columnCount;
        }
        /// <summary>
        /// Gets whether this range contains this column.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool ContainsColumn(int column)
        {
            return column >= LeftColumn && column <= RightColumn;
        }
        /// <summary>
        /// Gets whether this range contains this row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ContainsRow(int row)
        {
            return row >= TopRow && row <= BottomRow;
        }
        /// <summary>
        /// Gets whether this range contains this cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool ContainsCell(int row, int column)
        {
            return ContainsColumn(column) && ContainsRow(row);
        }
        /// <summary>
        /// Gets whether this range contains the provided range.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public bool ContainsRange(CellRange range)
        {
            return TopRow <= range.TopRow && BottomRow >= range.BottomRow
                && LeftColumn <= range.LeftColumn && RightColumn >= range.RightColumn;
        }

        public bool Intersects(CellRange range)
        {
            return TopRow <= range.TopRow || BottomRow >= range.BottomRow
                || LeftColumn <= range.LeftColumn || RightColumn >= range.RightColumn;
        }

        public override string ToString()
        {
            return $"TopRow:{TopRow}, BottomRow:{BottomRow}, LeftColumn:{LeftColumn}, RightColumn:{RightColumn}";
        }
        public CellRange Clone()
        {
            return new CellRange(TopRow, LeftColumn, RowCount, ColumnCount);
        }
    }
}
