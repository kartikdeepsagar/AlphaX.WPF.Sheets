using System;
using System.Collections.Generic;
using System.Linq;

namespace DevBrewLabs.Spreadsheet
{
    internal class RowHeaderColumns : SheetDimensionCollection<IColumn>, IColumns, IDisposable
    {
        private WorkSheet _workSheet;

        public IColumn this[string address]
        {
            get
            {
                return this[Extensions.GetColumnIndex(address)];
            }
        }

        public RowHeaders RowHeaders { get; }
        protected override LocationCache<IColumn> LocationCache { get; }

        public RowHeaderColumns(RowHeaders parent) : base()
        {
            RowHeaders = parent;
            _workSheet = parent.WorkSheet;

            LocationCache = new LocationCache<IColumn>(
                () => _workSheet.RowHeaders.ColumnCount,
                () => _workSheet.RowHeaders.DefaultColumnWidth,
                InternalCollection,
                c => c.Width);
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
            InternalCollection.Clear();
            InternalCollection = null;
        }

        public override void Insert(int index, int count)
        {
           
        }

        public override void Remove(int index, int count)
        {
            
        }
    }
}