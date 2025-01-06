using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaX.Sheets
{
    public class Columns : CollectionBase<Column>, IColumns
    {
        private Dictionary<int, double> _locationMap;
        protected override int Count
        {
            get
            {
                return GetCount();
            }
        }

        public Column this[string address]
        {
            get
            {               
                return this[Extensions.GetColumnIndex(address)];
            }
        }

        internal Columns(object parent) : base(parent)
        {
            _locationMap = new Dictionary<int, double>();
        }

        /// <summary>
        /// Gets the location of the column.
        /// </summary>
        /// <param name="index">
        /// Column index.
        /// </param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal override double GetLocation(int column, bool recalculate = false)
        {
            if (_locationMap.ContainsKey(column) && !recalculate)
                return _locationMap[column];

            double xLocation = 0;
            double deltaWidth = 0;
            int count = 0;

            for (int index = column - 1; index >= 0; index--)
            {
                InternalCollection.TryGetValue(index, out var sheetColumn);

                if (sheetColumn != null)
                    deltaWidth += sheetColumn.Width;
                else
                    count++;

                if (_locationMap.ContainsKey(index))
                {
                    xLocation = _locationMap[index];
                    break;
                }
            }

            var defaultColumnWidth = 0;

            if (Parent is IWorkSheet workSheet)
            {
                defaultColumnWidth = workSheet.DefaultColumnWidth;
            }
            else if (Parent is IRowHeaders rowHeaders)
            {
                defaultColumnWidth = rowHeaders.DefaultColumnWidth;
            }
            else if (Parent is IColumnHeaders columnHeaders)
            {
                defaultColumnWidth = columnHeaders.WorkSheet.DefaultColumnWidth;
            }

            var location = xLocation + (count * defaultColumnWidth) + deltaWidth;

            if (!_locationMap.ContainsKey(column))
                _locationMap.Add(column, location);
            else
                _locationMap[column] = location;

            return location;
        }

        internal void UpdateColumnsLocation(int fromColumn, double offset)
        {
            for (int index = fromColumn; index < Count; index++)
            {
                if (_locationMap.ContainsKey(index))
                    _locationMap[index] += offset;
            }
        }

        protected override Column CreateItem(int index)
        {
            var column =  new Column(this);
            column.Index = index;
            return column;
        }

        public int GetColumnWidth(int column)
        {
            var col = GetItem(column, false);

            if (col == null)
                return GetDefaultColumnWidth();
            
            return col.Width;
        }

        private int GetDefaultColumnWidth()
        {
            if (Parent is IWorkSheet workSheet)
            {
                return workSheet.DefaultColumnWidth;
            }
            else if (Parent is IRowHeaders rowHeaders)
            {
                return rowHeaders.DefaultColumnWidth;
            }
            else if (Parent is IColumnHeaders columnHeaders)
            {
                return columnHeaders.WorkSheet.DefaultColumnWidth;
            }

            return 0;
        }

        private int GetCount()
        {
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

        public int GetColumnIndex(IColumn column)
        {
            var result = InternalCollection.FirstOrDefault(x => x.Value == column);
            return result.Key;
        }

        public void Dispose()
        {
            _locationMap.Clear();
            InternalCollection.Clear();
            InternalCollection = null;
            _locationMap = null;
        }

        public override void InsertBelow(int index)
        {
            InsertBelow(index, 1);
        }

        public override void Add()
        {
            Add(1);
        }

        public override void InsertBelow(int index, int count)
        {
            if (Parent is IWorkSheet workSheet)
            {
                foreach(var item in InternalCollection.ToList())
                {
                    if (item.Key < index)
                        continue;

                    InternalCollection.Remove(item.Key);
                    InternalCollection.Add(item.Key + count, item.Value);
                }

                workSheet.ColumnCount += count;
            }
        }

        public override void Add(int count)
        {
            if (Parent is WorkSheet workSheet)
            {
                workSheet.ColumnCount += count;
            }
        }

        public override void Remove(int index)
        {
            throw new NotImplementedException();
        }

        public override void Remove(int index, int count)
        {
            throw new NotImplementedException();
        }
    }
}
