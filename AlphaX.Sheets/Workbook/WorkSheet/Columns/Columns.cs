using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaX.Sheets
{
    internal class Columns : CollectionBase<IColumn>, IColumns
    {
        private Dictionary<int, double> _locationMap;
        
        protected override int Count
        {
            get
            {
                return GetCount();
            }
        }

        public IColumn this[string address]
        {
            get
            {               
                return this[Extensions.GetColumnIndex(address)];
            }
        }

        public SheetRegion Region { get; }
        public WorkSheet WorkSheet { get; }
        public RowHeaders RowHeaders { get; }
        public ColumnHeaders ColumnHeaders { get; }

        private Columns() : base()
        {
            _locationMap = new Dictionary<int, double>();
        }

        internal Columns(WorkSheet parent) : this()
        {
            WorkSheet = parent;
            Region = SheetRegion.Cells;
        }

        internal Columns(RowHeaders parent) : this()
        {
            RowHeaders = parent;
            WorkSheet = (WorkSheet)parent.WorkSheet;
            Region = SheetRegion.RowHeader;
        }

        internal Columns(ColumnHeaders parent) : this()
        {
            ColumnHeaders = parent;
            WorkSheet = (WorkSheet)parent.WorkSheet;
            Region = SheetRegion.ColumnHeader;
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
                double defWidth = GetDefaultColumnWidth();
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

            var defaultColumnWidth = GetDefaultColumnWidth();

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

        protected override IColumn CreateItem(int index)
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
            switch (Region)
            {
                case SheetRegion.Cells:
                case SheetRegion.ColumnHeader:
                    return WorkSheet.DefaultColumnWidth;

                case SheetRegion.RowHeader:
                    return RowHeaders.DefaultColumnWidth;
            }

            return 0;
        }

        private int GetCount()
        {
            switch (Region)
            {
                case SheetRegion.Cells:
                case SheetRegion.ColumnHeader:
                    return WorkSheet.ColumnCount;

                case SheetRegion.RowHeader:
                    return RowHeaders.ColumnCount;
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
            switch (Region)
            {
                case SheetRegion.Cells:
                    var items = InternalCollection.ToList();

                    for (int itemIndex = items.Count - 1; itemIndex >= 0; itemIndex--)
                    {
                        var item = items[itemIndex];

                        if (item.Key >= index)
                        {
                            InternalCollection.Remove(item.Key);
                            InternalCollection.Add(item.Key + count, item.Value);
                        }
                    }

                    WorkSheet.ColumnCount += count;
                    break;
            }
        }

        public override void Add(int count)
        {
            switch (Region)
            {
                case SheetRegion.Cells:
                    WorkSheet.ColumnCount += count;
                    break;
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
