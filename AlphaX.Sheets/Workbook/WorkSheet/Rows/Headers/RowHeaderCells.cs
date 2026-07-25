using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlphaX.Sheets
{
    internal class RowHeaderCells : IRange, IDisposable
    {
        private int _rowCount;
        private int _columnCount;
        private WorkSheet _workSheet;
        private RowHeaders _rowHeaders;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<long, RowHeaderCell> _activeCellInstances;

        public IRange this[string name]
        {
            get
            {
                throw new NotSupportedException();
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
                if (_rowCount == -1)
                    return _workSheet.RowCount;

                return _rowCount;
            }
        }

        public int ColumnCount
        {
            get
            {
                if (_columnCount == -1)
                    return _rowHeaders.ColumnCount;

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
                ApplyToRange((range) => ((RowHeaderCell)range).IsVisible = value);
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

        public WorkSheet WorkSheet => _workSheet;
        public RowHeaders RowHeaders => _rowHeaders;

        internal RowHeaderCells(RowHeaders parent)
        {
            _rowHeaders = parent;
            _workSheet = parent.WorkSheet;
            Row = Column = 0;
            _rowCount = _columnCount = -1;
            _activeCellInstances = new Dictionary<long, RowHeaderCell>();
        }

        internal RowHeaderCells(RowHeaderCells parentRange, int row, int column, int rowCount, int columnCount)
        {
            _workSheet = parentRange._workSheet;
            ParentRange = parentRange;
            Row = row;
            Column = column;
            _rowCount = rowCount;
            _columnCount = columnCount;
            _activeCellInstances = parentRange._activeCellInstances;
            _workSheet = parentRange._workSheet;
        }

        internal RowHeaderCell GetCell(int row, int column, bool createIfNotExists)
        {
            ValidateIndexes(row, column, 1, 1);
            long key = MakeKey(row, column);

            if (_activeCellInstances.TryGetValue(key, out var existingCell))
            {
                existingCell.Row = row;
                existingCell.Column = column;
                return existingCell;
            }

            var colData = _workSheet.GetColumnData(column, false);
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
            var colData = _workSheet.GetColumnData(column, false);
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

        internal void ClearCellStore()
        {
            _activeCellInstances.Clear();
        }

        internal void ClearColumnCells(int column)
        {
            var columnCells = _activeCellInstances.Where(x => GetColumn(x.Key) == column).ToList();

            foreach (var cell in columnCells)
            {
                _activeCellInstances.Remove(cell.Key);
            }
        }

        private RowHeaderCells GetRange(int row, int column, int rowCount, int columnCount)
        {
            ValidateIndexes(row, column, rowCount, columnCount);
            return new RowHeaderCells(this, row, column, rowCount, columnCount);
        }

        private RowHeaderCell CreateCell(int row, int column)
        {
            long key = MakeKey(row, column);
            if (_activeCellInstances.TryGetValue(key, out var cell))
            {
                cell.Row = row;
                cell.Column = column;
                return cell;
            }

            cell = new RowHeaderCell(this)
            {
                Row = row,
                Column = column
            };

            _activeCellInstances[key] = cell;
            return cell;
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

        public void Dispose()
        {
            ClearCellStore();
        }

        private static long MakeKey(int row, int column)
        {
            return ((long)row << 32) | (uint)column;
        }

        private static int GetRow(long key)
        {
            return (int)(key >> 32);
        }

        private static int GetColumn(long key)
        {
            return (int)key;
        }
    }
}
