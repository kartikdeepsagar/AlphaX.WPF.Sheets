using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaX.Sheets
{
    internal class RowHeaderColumns : CollectionBase<IColumn>, IColumns, IDisposable
    {
        private Dictionary<int, double> _locationMap;

        public IColumn this[string address]
        {
            get
            {
                return this[Extensions.GetColumnIndex(address)];
            }
        }

        public RowHeaders RowHeaders { get; }

        public RowHeaderColumns(RowHeaders parent) : base()
        {
            _locationMap = new Dictionary<int, double>();
            RowHeaders = parent;
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

            if (InternalCollection.Count == 0)
            {
                double defWidth = RowHeaders.DefaultColumnWidth;
                double loc = column * defWidth;
                _locationMap[column] = loc;
                return loc;
            }

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

            var location = xLocation + (count * RowHeaders.DefaultColumnWidth) + deltaWidth;

            if (!_locationMap.ContainsKey(column))
                _locationMap.Add(column, location);
            else
                _locationMap[column] = location;

            return location;
        }

        internal void UpdateColumnsLocation(int fromColumn, double offset)
        {
            for (int index = fromColumn; index < RowHeaders.ColumnCount; index++)
            {
                if (_locationMap.ContainsKey(index))
                    _locationMap[index] += offset;
            }
        }

        protected override IColumn CreateItem(int index)
        {
            var column = new RowHeaderColumn(this);
            column.Index = index;
            return column;
        }

        public int GetColumnWidth(int column)
        {
            var col = GetItem(column, false);

            if (col == null)
                return RowHeaders.DefaultColumnWidth;

            return col.Width;
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

        public override void Insert(int index, int count)
        {
           
        }

        public override void Remove(int index, int count)
        {
            
        }
    }
}