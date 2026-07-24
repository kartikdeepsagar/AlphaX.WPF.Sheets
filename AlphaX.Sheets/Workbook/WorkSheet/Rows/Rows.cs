using System.Collections.Generic;
using System.Linq;

namespace AlphaX.Sheets
{
    internal class Rows : CollectionBase<IRow>, IRows
    {
        private Dictionary<int, double> _locationMap;

        protected override int Count
        {
            get
            {
                return GetCount();
            }
        }

        public SheetRegion Region { get; }
        public WorkSheet WorkSheet { get; }
        public RowHeaders RowHeaders { get; }
        public ColumnHeaders ColumnHeaders { get; }

        private Rows() : base()
        {
            _locationMap = new Dictionary<int, double>();
        }

        internal Rows(WorkSheet parent) : this()
        {
            WorkSheet = parent;
            Region = SheetRegion.Cells;
        }

        internal Rows(RowHeaders parent) : this()
        {
            RowHeaders = parent;
            WorkSheet = (WorkSheet)parent.WorkSheet;
            Region = SheetRegion.RowHeader;
        }

        internal Rows(ColumnHeaders parent) : this()
        {
            ColumnHeaders = parent;
            WorkSheet = (WorkSheet)parent.WorkSheet;
            Region = SheetRegion.ColumnHeader;
        }

        /// <summary>
        /// Gets the location of the row
        /// </summary>
        /// <param name="row">
        /// Row index.
        /// </param>
        /// <returns></returns>
        internal override double GetLocation(int row, bool recalculate = false)
        {
            if (_locationMap.ContainsKey(row) && !recalculate)
                return _locationMap[row];

            if (InternalCollection.Count == 0)
            {
                double defHeight = GetDefaultRowHeight();
                double loc = row * defHeight;
                _locationMap[row] = loc;
                return loc;
            }

            double yLocation = 0;
            double deltaHeight = 0;
            int count = 0;

            for(int index = row - 1; index >= 0; index--)
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

            var defaultRowHeight = GetDefaultRowHeight();
 
            var location = yLocation + (count * defaultRowHeight) + deltaHeight;

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
            var row =  new Row(this);
            row.Index = index;
            return row;
        }

        public int GetRowHeight(int row)
        {
            var sheetRow = GetItem(row, false);

            if (sheetRow == null)
                return GetDefaultRowHeight();

            return sheetRow.Height;
        }

        private int GetDefaultRowHeight()
        {
            switch (Region)
            {
                case SheetRegion.Cells:
                case SheetRegion.RowHeader:
                    return WorkSheet.DefaultRowHeight;

                case SheetRegion.ColumnHeader:
                    return ColumnHeaders.DefaultRowHeight;
            }

            return 0;
        }

        private int GetCount()
        {
            switch (Region)
            {
                case SheetRegion.Cells:
                case SheetRegion.RowHeader:
                   return WorkSheet.RowCount;

                case SheetRegion.ColumnHeader:
                    return ColumnHeaders.RowCount;
            }

            return 0;
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

        public override void Remove(int index)
        {
            Remove(index, 1);
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

                    ((Cells)WorkSheet.Cells).InsertRows(index, count);
                    WorkSheet.RowCount += count;
                    WorkSheet.OnRowsChanged(new RowChangedEventArgs(SheetRegion.Cells, index, count, RowChangeType.Insert));
                    break;
            }
        }

        public override void Add(int count)
        {
            switch(Region)
            {
                case SheetRegion.Cells:
                    WorkSheet.RowCount += count;
                    break;
            }
        }

        public override void Remove(int index, int count)
        {
            switch (Region)
            {
                case SheetRegion.Cells:
                    var items = InternalCollection.ToList();

                    for (int itemIndex = 0; itemIndex < items.Count - 1; itemIndex++)
                    {
                        var item = items[itemIndex];

                        if (item.Key >= index && item.Key < index + count)
                        {
                            InternalCollection.Remove(item.Key);
                        }
                        else if (item.Key >= index + count)
                        {
                            InternalCollection.Remove(item.Key);
                            InternalCollection.Add(item.Key - count, item.Value);
                        }
                    }

                    ((Cells)WorkSheet.Cells).RemoveRows(index, count);
                    WorkSheet.RowCount -= count;
                    WorkSheet.OnRowsChanged(new RowChangedEventArgs(SheetRegion.Cells, index, count, RowChangeType.Delete));
                    break;
            }
        }
    }
}
