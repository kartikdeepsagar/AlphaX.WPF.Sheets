namespace AlphaX.Sheets
{
    internal class ColumnHeaders : HeadersBase, IColumnHeaders
    {
        private ColumnHeaderCells _cells;
        private ColumnHeaderRows _rows;

        public int RowCount { get; set; }
        public int DefaultRowHeight { get; set; }
        public double Height
        {
            get
            {
                var row = _rows.GetItem(RowCount - 1);
                var rowLocation = _rows.GetLocation(RowCount - 1);

                if (row == null)
                    return rowLocation + DefaultRowHeight;

                return rowLocation + row.Height;
            }
        }

        public IRange Cells => _cells;
        public IRows Rows => _rows;

        internal ColumnHeaders(WorkSheet workSheet) : base(workSheet)
        {
            RowCount = 1;
            DefaultRowHeight = 20;
            _cells = new ColumnHeaderCells(this);
            _rows = new ColumnHeaderRows(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            _cells.Dispose();
            _rows.Dispose();
        }
    }
}
