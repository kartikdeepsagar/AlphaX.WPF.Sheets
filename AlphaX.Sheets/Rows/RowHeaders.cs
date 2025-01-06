namespace AlphaX.Sheets
{
    public class RowHeaders : HeadersBase, IRowHeaders
    {
        public int ColumnCount { get; set; }
        public int DefaultColumnWidth { get; set; }
        public double Width
        {
            get
            {
                var column = Columns.GetItem(ColumnCount - 1, false);
                var columnLocation = Columns.GetLocation(ColumnCount - 1);

                if (column == null)
                    return columnLocation + DefaultColumnWidth;

                return columnLocation + column.Width;
            }
        }

        internal RowHeaders(WorkSheet workSheet) : base(workSheet)
        {
            DefaultColumnWidth = 30;
            ColumnCount = 1;
        }
    }
}
