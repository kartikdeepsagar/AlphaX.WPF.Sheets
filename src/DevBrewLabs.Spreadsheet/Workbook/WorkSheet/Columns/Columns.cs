using System;
using System.Collections.Generic;
using System.Linq;

namespace DevBrewLabs.Spreadsheet
{
    internal class Columns : SheetDimensionCollection<IColumn>, IColumns, IDisposable
    {
        public IColumn this[string address]
        {
            get
            {               
                return this[Extensions.GetColumnIndex(address)];
            }
        }

        public WorkSheet WorkSheet { get; }
        protected override LocationCache<IColumn> LocationCache { get; }

        internal Columns(WorkSheet parent) : base()
        {
            WorkSheet = parent;

            LocationCache = new LocationCache<IColumn>(
                () => WorkSheet.ColumnCount,
                () => WorkSheet.DefaultColumnWidth,
                InternalCollection,
                c => c.Width);
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
                return WorkSheet.DefaultColumnWidth;
            
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
