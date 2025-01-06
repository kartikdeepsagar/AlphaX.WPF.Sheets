namespace AlphaX.Sheets
{
    public class ColumnHeaders : HeadersBase, IColumnHeaders
    {
        public int RowCount { get; set; }
        public int DefaultRowHeight { get; set; }
        public double Height
        {
            get
            {
                var row = Rows.GetItem(RowCount - 1, false);
                var rowLocation = Rows.GetLocation(RowCount - 1);

                if (row == null)
                    return rowLocation + DefaultRowHeight;

                return rowLocation + row.Height;
            }
        }

        internal ColumnHeaders(WorkSheet workSheet) : base(workSheet)
        {
            RowCount = 1;
            DefaultRowHeight = 20;
        }
    }
}
