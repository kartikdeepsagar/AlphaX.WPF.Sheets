namespace AlphaX.Sheets
{
    internal class ColumnHeaders : HeadersBase, IColumnHeaders
    {
        public int RowCount { get; set; }
        public int DefaultRowHeight { get; set; }
        public double Height
        {
            get
            {
                var row = _rows.GetItem(RowCount - 1, false);
                var rowLocation = _rows.GetLocation(RowCount - 1);

                if (row == null)
                    return rowLocation + DefaultRowHeight;

                return rowLocation + row.Height;
            }
        }

        internal ColumnHeaders(WorkSheet workSheet) : base(workSheet)
        {
            RowCount = 1;
            DefaultRowHeight = 20;
            _cells = new Cells(this);
            _rows = new Rows(this);
            _columns = new Columns(this);
        }
    }
}
