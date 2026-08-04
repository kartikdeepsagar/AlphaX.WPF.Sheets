using System;
using System.Linq;

namespace DevBrewLabs.Spreadsheet
{
    internal class ColumnHeaderRows : SheetDimensionCollection<IRow>, IRows, IDisposable
    {
        private WorkSheet _workSheet;

        public ColumnHeaders ColumnHeaders { get; }
        protected override LocationCache<IRow> LocationCache { get; }

        internal ColumnHeaderRows(ColumnHeaders parent) : base()
        {
            ColumnHeaders = parent;
            _workSheet = parent.WorkSheet;

            LocationCache = new LocationCache<IRow>(
                () => _workSheet.ColumnHeaders.RowCount,
                () => _workSheet.ColumnHeaders.DefaultRowHeight,
                InternalCollection,
                r => r.Height);
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
            InternalCollection.Clear();
            InternalCollection = null;
        }
    }
}
