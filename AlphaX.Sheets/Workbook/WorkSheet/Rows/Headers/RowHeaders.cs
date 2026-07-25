namespace AlphaX.Sheets
{
    internal class RowHeaders : HeadersBase, IRowHeaders
    {
        private RowHeaderCells _cells;
        private RowHeaderColumns _columns;

        public int ColumnCount { get; set; }
        public int DefaultColumnWidth { get; set; }
        public double Width
        {
            get
            {
                var column = _columns.GetItem(ColumnCount - 1);
                var columnLocation = _columns.GetLocation(ColumnCount - 1);

                if (column == null)
                    return columnLocation + DefaultColumnWidth;

                return columnLocation + column.Width;
            }
        }

        public IRange Cells => _cells;
        public IColumns Columns => _columns;

        internal RowHeaders(WorkSheet workSheet) : base(workSheet)
        {
            DefaultColumnWidth = 30;
            ColumnCount = 1;
            _cells = new RowHeaderCells(this);
            _columns = new RowHeaderColumns(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            _cells.Dispose();
            _columns.Dispose();
        }
    }
}
