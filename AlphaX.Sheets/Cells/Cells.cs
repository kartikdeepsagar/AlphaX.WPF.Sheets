using AlphaX.CalcEngine.Parsers;
using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaX.Sheets
{
    public class Cells : ICell
    {
        static Cells()
        {
            _sortComparer = new NaturalSortComparer();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static NaturalSortComparer _sortComparer;
        private int _rowCount;
        private int _columnCount;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SortedDictionary<int, SortedDictionary<int, Cell>> _cellStore;

        public Cells this[string name]
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

        public Cell this[int row, int column]
        {
            get
            {
                return GetCell(row, column, true);
            }
        }

        public Cells this[int row, int column, int rowCount, int columnCount]
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
                if (_rowCount >= 0)
                    return _rowCount;

                if (Parent is IWorkSheet workSheet)
                {
                    return workSheet.RowCount;
                }
                else if (Parent is IRowHeaders rowHeaders)
                {
                    return rowHeaders.WorkSheet.RowCount;
                }
                else if (Parent is IColumnHeaders columnHeaders)
                {
                    return columnHeaders.RowCount;
                }

                return 0;
            }
        }

        public int ColumnCount
        {
            get
            {
                if (_columnCount >= 0)
                    return _columnCount;

                if (Parent is IWorkSheet workSheet)
                {
                    return workSheet.ColumnCount;
                }
                else if (Parent is IRowHeaders rowHeaders)
                {
                    return rowHeaders.ColumnCount;
                }
                else if (Parent is IColumnHeaders columnHeaders)
                {
                    return columnHeaders.WorkSheet.ColumnCount;
                }

                return 0;
            }
        }

        public object Value
        {
            get
            {
                return GetCell(Row, Column, true).Value;
            }
            set
            {
                ApplyToRange(x => x.Value = value);
            }
        }

        public string Formula
        {
            get
            {
                var cell = GetCell(Row, Column, false);
                if (cell != null)
                    return cell.Formula;
                else
                    return null;
            }
            set
            {
                ApplyToRange(x => x.Formula = value);
            }
        }

        public IFormatter Formatter
        {
            get
            {
                var cell = GetCell(Row, Column, false);
                if (cell != null)
                    return cell.Formatter;
                else
                    return null;
            }
            set
            {
                ApplyToRange(x => x.Formatter = value);
            }
        }

        public string StyleName
        {
            get
            {
                return GetCell(Row, Column, true).StyleName;
            }
            set
            {
                ApplyToRange(x => x.StyleName = value);
            }
        }

        public object Parent { get; }

        public DataMap DataMap
        {
            get
            {
                return GetCell(Row, Column, true).DataMap;
            }
            set
            {
                ApplyToRange(x => x.DataMap = value);
            }
        }

        public ICellType CellType
        {
            get
            {
                return GetCell(Row, Column, true).CellType;
            }
            set
            {
                ApplyToRange(x => x.CellType = value);
            }
        }

        Cells ICell.Parent { get; }
        public bool HasFormula => false;

        public bool Locked
        {
            get
            {
                return GetCell(Row, Column, true).Locked;
            }
            set
            {
                ApplyToRange(x => x.Locked = value);
            }
        }

        public bool IsVisible
        {
            get
            {
                return GetCell(Row, Column, true).IsVisible;
            }
            set
            {
                ApplyToRange(x => x.IsVisible = value);
            }
        }

        public int RowSpan
        {
            get
            {
                return GetCell(Row, Column, true).RowSpan;
            }
            set
            {
                ApplyToRange(x => x.RowSpan = value);
            }
        }

        public int ColumnSpan
        {
            get
            {
                return GetCell(Row, Column, true).ColumnSpan;
            }
            set
            {
                ApplyToRange(x => x.ColumnSpan = value);
            }
        }

        internal Cells(object parent)
        {
            Parent = parent;
            Row = Column = 0;
            _rowCount = _columnCount = -1;
            _cellStore = new SortedDictionary<int, SortedDictionary<int, Cell>>();
        }

        internal Cells(Cells parentRange, int row, int column, int rowCount, int columnCount)
        {
            Parent = parentRange.Parent;
            Row = row;
            Column = column;
            _rowCount = rowCount;
            _columnCount = columnCount;
            _cellStore = parentRange._cellStore;
        }

        /// <summary>
        /// Gets the cell present at row/column index.
        /// </summary>
        /// <param name="row">
        /// Row index.
        /// </param>
        /// <param name="column">
        /// Column index.
        /// </param>
        /// <param name="createIfNotExists">
        /// Whether to create the cell if not present in cell store.
        /// </param>
        /// <returns></returns>
        internal Cell GetCell(int row, int column, bool createIfNotExists)
        {
            ValidateIndexes(row, column, 1, 1);
            Cell cell = null;
            if (ContainsCell(row, column))
            {
                cell = _cellStore[row][column];
                cell.Row = row;
                cell.Column = column;
            }
            else if (createIfNotExists)
            {
                cell = CreateCell(row, column);
                cell.Row = row;
                cell.Column = column;
            }

            return cell;
        }

        /// <summary>
        /// Gets the column cells present in cell store.
        /// </summary>
        /// <param name="column"></param>
        internal IEnumerable<KeyValuePair<int, object>> GetCellValues(int column)
        {
            foreach(var rowCells in _cellStore)
            {
                if(rowCells.Value.ContainsKey(column) && rowCells.Value[column].Value != null)
                {
                    yield return new KeyValuePair<int, object>(rowCells.Key, rowCells.Value[column].Value);
                }
            }
        }

        internal void ClearColumnCells(int column)
        {
            foreach (var rowCells in _cellStore)
            {
                if (rowCells.Value.ContainsKey(column))
                {
                    rowCells.Value[column] = null;
                    rowCells.Value.Remove(column);
                }
            }
        }

        public async void Sort(bool ascending)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int col = Column; col < Column + ColumnCount; col++)
                {
                    var cells = new Dictionary<int, Cell>();

                    for (int row = Row; row < Row + RowCount; row++)
                    {
                        cells.Add(row, GetCell(row, col, false));
                    }

                    var sortedCells = cells.ToList();
                    sortedCells.Sort(_sortComparer);

                    if (!ascending)
                        sortedCells.Reverse();

                    var enumerator = cells.GetEnumerator();

                    foreach (var cell in sortedCells)
                    {
                        enumerator.MoveNext();
                        var current = enumerator.Current;
                        MoveCell(cell.Value, current.Key, col);
                    }
                }
            });

            if (Parent is WorkSheet workSheet)
            {
                workSheet.OnRangeChanged(new RangeChangedEventArgs()
                {
                    Action = SheetAction.Sort,
                    ChangeType = ChangeType.None,
                    SortState = ascending ? SortState.Ascending : SortState.Descending,
                    CellRange = new CellRange(Row, Column, RowCount, ColumnCount)
                });
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

            if (Parent is WorkSheet workSheet)
            {
                workSheet.OnRangeChanged(new RangeChangedEventArgs()
                {
                    ChangeType = ChangeType.None,
                    Action = SheetAction.Merge,
                    CellRange = new CellRange(Row, Column, RowCount, ColumnCount)
                });
            }
        }

        internal void ClearCellStore()
        {
            foreach (var item in _cellStore)
            {
                foreach(var value in item.Value)
                {
                    value.Value.Value = null;
                }
                item.Value.Clear();
            }
                
            _cellStore.Clear();
        }

        internal void InsertRows(int index, int count)
        {
            if (Parent is WorkSheet)
            {
                var items = _cellStore.ToList();

                for (int itemIndex = items.Count - 1; itemIndex >= 0; itemIndex--)
                {
                    var item = items[itemIndex];

                    if (item.Key >= index)
                    {
                        _cellStore.Remove(item.Key);
                        _cellStore.Add(item.Key + count, item.Value);
                    }
                }
            }
        }

        internal void RemoveRows(int index, int count)
        {
            if (Parent is WorkSheet workSheet)
            {
                var items = _cellStore.ToList();

                for (int itemIndex = 0; itemIndex < items.Count; itemIndex++)
                {
                    var item = items[itemIndex];

                    if (item.Key >= index && item.Key < index + count)
                    {
                        var rowCells = _cellStore[item.Key];

                        foreach (var cell in rowCells)
                        {
                            workSheet.DataStore.SetValue(item.Key, cell.Key, null);
                        }

                        rowCells.Clear();
                        _cellStore.Remove(item.Key);
                    }
                    else if (item.Key >= index + count)
                    {
                        _cellStore.Remove(item.Key);
                        _cellStore.Add(item.Key - count, item.Value);
                    }
                }
            }
        }

        private void MoveCell(Cell cell, int toRow, int toColumn)
        {
            if (cell?.Row == toRow && cell?.Column == toColumn)
                return;

            if(_cellStore.ContainsKey(toRow))
            {
                if(cell == null)
                    _cellStore[toRow].Remove(toColumn);
                else
                {
                    if (_cellStore[toRow].ContainsKey(toColumn))
                        _cellStore[toRow].Remove(toColumn);

                    _cellStore[toRow].Add(toColumn, cell);
                }
                return;
            }

            if (cell != null)
            {
                _cellStore.Add(toRow, new SortedDictionary<int, Cell>());
                _cellStore[toRow].Add(toColumn, cell);
            }
        }

        /// <summary>
        /// Gets the cell range.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        private Cells GetRange(int row, int column, int rowCount, int columnCount)
        {
            ValidateIndexes(row, column, rowCount, columnCount);
            return new Cells(this, row, column, rowCount, columnCount);
        }

        /// <summary>
        /// Creates a new cell.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private Cell CreateCell(int row, int column)
        {
            var cell = new Cell(this);

            if (!_cellStore.ContainsKey(row))
                _cellStore.Add(row, new SortedDictionary<int, Cell>());

            _cellStore[row].Add(column, cell);
            return cell;
        }

        /// <summary>
        /// Gets whether the cell is present in the cell store or not.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool ContainsCell(int row, int column)
        {
            return _cellStore.ContainsKey(row) && _cellStore[row].ContainsKey(column);
        }

        /// <summary>
        /// Validates whether the indexes are out of range or not.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private void ValidateIndexes(int row, int column, int rowCount, int columnCount)
        {
            //if (row < Row || row >= Row + RowCount || column <  Column || column >= Column + ColumnCount
            //    || RowCount < rowCount || ColumnCount < columnCount)
            //    throw new IndexOutOfRangeException("Provided indexes doesn't belong to this cell range.");
        }

        /// <summary>
        /// Executes action for each cell present in range.
        /// </summary>
        /// <param name="action"></param>
        private void ApplyToRange(Action<ICell> action)
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
            _cellStore = null;
        }
    }
}
