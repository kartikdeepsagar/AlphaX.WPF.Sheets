namespace AlphaX.Sheets
{
    public abstract class HeadersBase : IHeaders
    {
        protected Columns _columns;
        protected Rows _rows;
        protected Cells _cells;
        protected WorkSheet _workSheet;

        public IRows Rows => _rows;
        public IColumns Columns => _columns;
        public IRange Cells => _cells;
        public IWorkSheet WorkSheet => _workSheet;

        internal HeadersBase(WorkSheet workSheet)
        {
            _workSheet = workSheet;
            _rows = new Rows(this);
            _columns = new Columns(this);
            _cells = new Cells(this);
        }

        public void Dispose()
        {
            _cells.Dispose();
            _columns.Dispose();
            _rows.Dispose();
        }
    }
}
