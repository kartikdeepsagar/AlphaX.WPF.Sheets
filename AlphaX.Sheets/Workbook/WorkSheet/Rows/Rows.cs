using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaX.Sheets
{
    internal class Rows : SheetDimensionCollection<IRow>, IRows, IDisposable
    {
        public WorkSheet WorkSheet { get; }
        protected override LocationCache<IRow> LocationCache { get; }

        internal Rows(WorkSheet parent) : base()
        {
            WorkSheet = parent;
            LocationCache = new LocationCache<IRow>(
                () => WorkSheet.RowCount,
                () => WorkSheet.DefaultRowHeight,
                InternalCollection,
                r => r.Height);
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
                return WorkSheet.DefaultRowHeight;

            return sheetRow.Height;
        }


        public override void Insert(int index, int count)
        {

        }

        public override void Remove(int index, int count)
        {
            
        }

        public void Dispose()
        {
            InternalCollection.Clear();
            InternalCollection = null;
        }
    }
}
