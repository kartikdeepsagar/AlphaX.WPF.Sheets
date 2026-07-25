using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaX.Sheets
{
    internal class ColumnHeaderRows : CollectionBase<IRow>, IRows, IDisposable
    {
        private Dictionary<int, double> _locationMap;
        public ColumnHeaders ColumnHeaders { get; }

        internal ColumnHeaderRows(ColumnHeaders parent) : base()
        {
            ColumnHeaders = parent;
            _locationMap = new Dictionary<int, double>();
        }

        /// <summary>
        /// Gets the location of the row
        /// </summary>
        /// <param name="row">
        /// Row index.
        /// </param>
        /// <param name="recalculate">
        /// Skip cache.
        /// </param>
        /// <returns></returns>
        internal override double GetLocation(int row, bool recalculate = false)
        {
            if (_locationMap.ContainsKey(row) && !recalculate)
                return _locationMap[row];

            if (InternalCollection.Count == 0)
            {
                double defHeight = ColumnHeaders.DefaultRowHeight;
                double loc = row * defHeight;
                _locationMap[row] = loc;
                return loc;
            }

            double yLocation = 0;
            double deltaHeight = 0;
            int count = 0;

            for (int index = row - 1; index >= 0; index--)
            {
                InternalCollection.TryGetValue(index, out var sheetRow);

                if (sheetRow != null)
                    deltaHeight += sheetRow.Height;
                else
                    count++;

                if (_locationMap.ContainsKey(index))
                {
                    yLocation = _locationMap[index];
                    break;
                }
            }

            var location = yLocation + (count * ColumnHeaders.DefaultRowHeight) + deltaHeight;

            if (!_locationMap.ContainsKey(row))
                _locationMap.Add(row, location);
            else
                _locationMap[row] = location;

            return location;
        }

        internal void UpdateRowsLocation(int fromRow, double offset)
        {
            _locationMap?.Clear();
        }

        protected override IRow CreateItem(int index)
        {
            var row = new ColumnHeaderRow(this);
            row.Index = index;
            return row;
        }

        public int GetRowHeight(int row)
        {
            var sheetRow = GetItem(row, false);

            if (sheetRow == null)
                return ColumnHeaders.DefaultRowHeight;

            return sheetRow.Height;
        }

        public int GetRowIndex(IRow row)
        {
            var result = InternalCollection.FirstOrDefault(x => x.Value == row);
            return result.Key;
        }

        public override void Insert(int index, int count)
        {

        }

        public override void Remove(int index, int count)
        {

        }

        public void Dispose()
        {
            _locationMap.Clear();
            InternalCollection.Clear();
            InternalCollection = null;
            _locationMap = null;
        }
    }
}
