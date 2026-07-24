namespace AlphaX.Sheets
{
    internal class RowHeaders : HeadersBase, IRowHeaders
    {
        public int ColumnCount { get; set; }
        public int DefaultColumnWidth { get; set; }
        public double Width
        {
            get
            {
                var column = _columns.GetItem(ColumnCount - 1, false);
                var columnLocation = _columns.GetLocation(ColumnCount - 1);

                if (column == null)
                    return columnLocation + DefaultColumnWidth;

                return columnLocation + column.Width;
            }
        }

        internal RowHeaders(WorkSheet workSheet) : base(workSheet)
        {
            DefaultColumnWidth = 30;
            ColumnCount = 1;
            _cells = new Cells(this);
            _rows = new Rows(this);
            _columns = new Columns(this);
        }
    }
}
