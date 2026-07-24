using AlphaX.CalcEngine.Parsers;
using AlphaX.Sheets.Core;
using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaX.Sheets
{
    internal class Cells : IRange
    {
        static Cells()
        {
            _sortComparer = new NaturalSortComparer();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static NaturalSortComparer _sortComparer;
        private int _rowCount;
        private int _columnCount;
        internal SheetRegion Region { get; }
        private WorkSheet _workSheet;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<int, ColumnData> _columnStore;
        private readonly Dictionary<string, Cell> _activeCellInstances;

        public IRange this[string name]
        {
            get
            {
                if(name.Contains(":"))
                {
                    var rangeRef = new CellRangeRef(name);
                    return GetRange(rangeRef.TopRow, rangeRef.LeftColumn, rangeRef.RowCount, rangeRef.ColumnCount);
                }
                else
                {
                    var cell = new CellRef(name);
                    return GetRange(cell.Row, cell.Column, 1, 1);
                }
            }
        }

        public IRange this[int row, int column]
        {
            get
            {
                return GetCell(row, column, true);
            }
        }

        public IRange this[int row, int column, int rowCount, int columnCount]
        {
            get
            {
                return GetRange(row, column, rowCount, columnCount);
            }
        }

        public int Row { get; }

        public int Column { get; }

        public int RowCount
        {
            get
            {
                if (_rowCount == -1 && Parent is WorkSheet sheet)
                    return sheet.RowCount;

                return _rowCount;
            }
        }

        public int ColumnCount
        {
            get
            {
                if (_columnCount == -1 && Parent is WorkSheet sheet)
                    return sheet.ColumnCount;

                return _columnCount;
            }
        }

        public object Value
        {
            get
            {
                return GetCell(Row, Column, false)?.Value;
            }
            set
            {
                ApplyToRange((range) => range.Value = value);
            }
        }

        public string Formula
        {
            get
            {
                return GetCell(Row, Column, false)?.Formula;
            }
            set
            {
                ApplyToRange((range) => range.Formula = value);
            }
        }

        public IFormatter Formatter
        {
            get
            {
                return GetCell(Row, Column, false)?.Formatter;
            }
            set
            {
                ApplyToRange((range) => range.Formatter = value);
            }
        }

        public string StyleName
        {
            get
            {
                return GetCell(Row, Column, false)?.StyleName;
            }
            set
            {
                ApplyToRange((range) => range.StyleName = value);
            }
        }

        public IStyle Style
        {
            get
            {
                return GetCell(Row, Column, false)?.Style;
            }
            set
            {
                ApplyToRange((range) => range.Style = value);
            }
        }

        public IRange ParentRange { get; private set; }
        internal object Parent { get; private set; }

        public DataMap DataMap
        {
            get
            {
                return GetCell(Row, Column, false)?.DataMap;
            }
            set
            {
                ApplyToRange((range) => range.DataMap = value);
            }
        }

        public ICellType CellType
        {
            get
            {
                return GetCell(Row, Column, false)?.CellType;
            }
            set
            {
                ApplyToRange((range) => range.CellType = value);
            }
        }

        public bool HasFormula => GetCell(Row, Column, false)?.HasFormula ?? false;

        public bool Locked
        {
            get
            {
                return GetCell(Row, Column, false)?.Locked ?? false;
            }
            set
            {
                ApplyToRange((range) => range.Locked = value);
            }
        }

        public bool IsVisible
        {
            get
            {
                return GetCell(Row, Column, false)?.IsVisible ?? true;
            }
            internal set
            {
                ApplyToRange((range) => ((Cell)range).IsVisible = value);
            }
        }

        public int RowSpan
        {
            get
            {
                return GetCell(Row, Column, false)?.RowSpan ?? 1;
            }
            set
            {
                ApplyToRange((range) => range.RowSpan = value);
            }
        }

        public int ColumnSpan
        {
            get
            {
                return GetCell(Row, Column, false)?.ColumnSpan ?? 1;
            }
            set
            {
                ApplyToRange((range) => range.ColumnSpan = value);
            }
        }

        private Cells()
        {
            Row = Column = 0;
            _rowCount = _columnCount = -1;
            _columnStore = new Dictionary<int, ColumnData>();
            _activeCellInstances = new Dictionary<string, Cell>();
        }

        internal Cells(RowHeaders parent) : this()
        {
            Parent = parent;
            Region = SheetRegion.RowHeader;
            _workSheet = (WorkSheet)parent.WorkSheet;
        }

        internal Cells(ColumnHeaders parent) : this()
        {
            Parent = parent;
            Region = SheetRegion.ColumnHeader;
            _workSheet = (WorkSheet)parent.WorkSheet;
        }

        internal Cells(WorkSheet parent) : this()
        {
            Parent = parent;
            Region = SheetRegion.Cells;
            _workSheet = parent;
        }

        internal Cells(Cells parentRange, int row, int column, int rowCount, int columnCount)
        {
            Parent = parentRange?.Parent;
            ParentRange = parentRange;
            Row = row;
            Column = column;
            _rowCount = rowCount;
            _columnCount = columnCount;
            _columnStore = parentRange._columnStore;
            _activeCellInstances = parentRange._activeCellInstances;
            Region = parentRange.Region;
            _workSheet = parentRange._workSheet;
        }

        internal ColumnData GetColumnData(int column, bool createIfNotExists = true)
        {
            if (_columnStore.TryGetValue(column, out var colData))
                return colData;

            if (createIfNotExists)
            {
                colData = new ColumnData(column);
                _columnStore[column] = colData;
                return colData;
            }

            return null;
        }

        internal Cell GetCell(int row, int column, bool createIfNotExists)
        {
            ValidateIndexes(row, column, 1, 1);
            string key = $"{row}:{column}";

            if (_activeCellInstances.TryGetValue(key, out var existingCell))
            {
                existingCell.Row = row;
                existingCell.Column = column;
                return existingCell;
            }

            var colData = GetColumnData(column, false);
            if (colData != null && colData.HasRowData(row))
            {
                var cell = CreateCell(row, column);
                return cell;
            }
            else if (createIfNotExists)
            {
                var cell = CreateCell(row, column);
                return cell;
            }

            return null;
        }

        internal IEnumerable<KeyValuePair<int, object>> GetCellValues(int column)
        {
            var colData = GetColumnData(column, false);
            if (colData != null)
            {
                for (int row = Row; row < Row + RowCount; row++)
                {
                    var val = colData.GetValue(row);
                    if (val != null)
                        yield return new KeyValuePair<int, object>(row, val);
                }
            }
        }

        internal void ClearColumnCells(int column)
        {
            var colData = GetColumnData(column, false);
            colData?.Clear();
        }

        public void Sort(bool ascending, int keyColumn, bool hasHeader = false, bool sortColumnOnly = false)
        {
            int startRow = Row;
            int totalRows = RowCount;
            int startCol = Column;
            int totalCols = ColumnCount;

            if (totalRows <= 1)
                return;

            int sortStartRow = hasHeader ? startRow + 1 : startRow;
            int sortRowCount = hasHeader ? totalRows - 1 : totalRows;

            if (sortRowCount <= 1)
                return;

            int targetStartCol = sortColumnOnly ? keyColumn : startCol;
            int targetEndCol = sortColumnOnly ? keyColumn : (startCol + totalCols - 1);

            List<RowSnapshot> snapshots = new List<RowSnapshot>(sortRowCount);

            for (int r = sortStartRow; r < sortStartRow + sortRowCount; r++)
            {
                object keyVal = null;
                if (Parent is WorkSheet ws)
                    keyVal = ws.DataStore.GetValue(r, keyColumn);

                if (keyVal == null)
                    keyVal = GetCell(r, keyColumn, false)?.Value;

                var snapshot = new RowSnapshot(r, keyVal);

                for (int c = targetStartCol; c <= targetEndCol; c++)
                {
                    var colData = GetColumnData(c, false);
                    if (colData != null)
                    {
                        var cellData = colData.GetCellData(r);
                        snapshot.Data[c] = cellData;
                    }
                }

                snapshots.Add(snapshot);
            }

            snapshots.Sort(new NaturalSortComparer(ascending));

            for (int i = 0; i < snapshots.Count; i++)
            {
                int targetRow = sortStartRow + i;
                var snapshot = snapshots[i];

                for (int c = targetStartCol; c <= targetEndCol; c++)
                {
                    var colData = GetColumnData(c, true);
                    if (snapshot.Data.TryGetValue(c, out var cellData))
                    {
                        colData.SetCellData(targetRow, cellData);

                        if (Parent is WorkSheet ws)
                        {
                            if (ws.DataSource != null)
                                ws.DataStore.SetValue(targetRow, c, cellData.Value);
                        }
                    }
                    else
                    {
                        colData.ClearRow(targetRow);
                        if (Parent is WorkSheet ws && ws.DataSource != null)
                            ws.DataStore.SetValue(targetRow, c, null);
                    }
                }
            }

            _workSheet.OnRangeChanged(new RangeChangedEventArgs(
                 Region,
                 new CellRange(sortStartRow, targetStartCol, sortRowCount, targetEndCol - targetStartCol + 1),
                 RangeChangeType.Sort
             ));
        }

        internal void Sort(bool ascending)
        {
            Sort(ascending, Column, false, false);
        }

        internal struct RowSnapshot
        {
            public int OriginalRow { get; }
            public object KeyValue { get; }
            public Dictionary<int, CellData> Data { get; }

            public RowSnapshot(int originalRow, object keyValue)
            {
                OriginalRow = originalRow;
                KeyValue = keyValue;
                Data = new Dictionary<int, CellData>();
            }
        }

        public void Merge(string mergeStyle = null)
        {
            for (int col = Column; col < Column + ColumnCount; col++)
            {
                for (int row = Row + RowCount - 2; row >= Row; row--)
                {
                    var cell = GetCell(row, col, true);
                    var previousCell = GetCell(row + 1, col, true);

                    if (cell.Value?.ToString() == previousCell.Value?.ToString())
                    {
                        cell.RowSpan = previousCell.RowSpan < 2 ? 2 : previousCell.RowSpan + 1;
                        cell.StyleName = mergeStyle;
                        previousCell.IsVisible = false;
                    }
                }
            }

            _workSheet.OnRangeChanged(new RangeChangedEventArgs(
                Region,
                new CellRange(Row, Column, RowCount, ColumnCount),
                RangeChangeType.Merge
            ));
        }

        internal void ClearCellStore()
        {
            foreach (var col in _columnStore.Values)
            {
                col.Clear();
            }

            _columnStore.Clear();
            _activeCellInstances.Clear();
        }

        internal void InsertRows(int index, int count)
        {
            if (Parent is WorkSheet)
            {
                foreach (var colData in _columnStore.Values)
                {
                    var itemsToShift = new List<KeyValuePair<int, CellData>>();
                    for (int r = index; r < index + 10000; r++)
                    {
                        if (colData.HasRowData(r))
                        {
                            itemsToShift.Add(new KeyValuePair<int, CellData>(r, colData.GetCellData(r)));
                        }
                    }

                    itemsToShift.Reverse();
                    foreach (var kvp in itemsToShift)
                    {
                        colData.ClearRow(kvp.Key);
                        colData.SetCellData(kvp.Key + count, kvp.Value);
                    }
                }
            }
        }

        internal void RemoveRows(int index, int count)
        {
            if (Parent is WorkSheet workSheet)
            {
                foreach (var colData in _columnStore.Values)
                {
                    for (int r = index; r < index + count; r++)
                    {
                        workSheet.DataStore.SetValue(r, colData.ColumnIndex, null);
                        colData.ClearRow(r);
                    }
                }
            }
        }

        private Cells GetRange(int row, int column, int rowCount, int columnCount)
        {
            ValidateIndexes(row, column, rowCount, columnCount);
            return new Cells(this, row, column, rowCount, columnCount);
        }

        private Cell CreateCell(int row, int column)
        {
            string key = $"{row}:{column}";
            if (_activeCellInstances.TryGetValue(key, out var cell))
            {
                cell.Row = row;
                cell.Column = column;
                return cell;
            }

            cell = new Cell(this)
            {
                Row = row,
                Column = column
            };

            _activeCellInstances[key] = cell;
            return cell;
        }

        private bool ContainsCell(int row, int column)
        {
            var colData = GetColumnData(column, false);
            return colData != null && colData.HasRowData(row);
        }

        private void ValidateIndexes(int row, int column, int rowCount, int columnCount)
        {
        }

        private void ApplyToRange(Action<IRange> action)
        {
            for (int row = Row; row < Row + RowCount; row++)
            {
                for (int column = Column; column < Column + ColumnCount; column++)
                {
                    var cell = GetCell(row, column, true);
                    action(cell);
                }
            }
        }

        internal WorkSheet GetWorkSheet()
        {
            return _workSheet;
        }

        internal void LoadData(object[,] data, int startRow = 0, int startCol = 0)
        {
            if (data == null)
                return;

            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            if (rows == 0 || cols == 0)
                return;

            for (int c = 0; c < cols; c++)
            {
                int colIndex = startCol + c;
                var colData = GetColumnData(colIndex, true);

                for (int r = 0; r < rows; r++)
                {
                    int rowIndex = startRow + r;
                    object val = data[r, c];
                    colData.SetValue(rowIndex, val);

                    if (_workSheet != null && _workSheet.DataSource != null)
                    {
                        _workSheet.DataStore.SetValue(rowIndex, colIndex, val);
                    }
                }
            }

            _workSheet.OnRangeChanged(new RangeChangedEventArgs(
                     SheetRegion.Cells,
                     new CellRange(startRow, startCol, rows, cols),
                      RangeChangeType.Value));
        }

        public void Dispose()
        {
            ClearCellStore();
            _columnStore = null;
        }
    }
}
