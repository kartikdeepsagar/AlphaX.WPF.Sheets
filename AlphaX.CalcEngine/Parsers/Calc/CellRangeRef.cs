using System;

namespace AlphaX.CalcEngine.Parsers
{
    internal class CellRangeRef
    {
        public CellRef Start { get; }
        public CellRef End { get; }
        public string Name => Start.Name + ":" + End.Name;

        public string SheetName => Start.SheetName;

        public int TopRow { get; private set; }
        public int BottomRow { get; private set; }
        public int LeftColumn { get; private set; }
        public int RightColumn { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public CellRangeRef(string cellRangeRef)
        {
            var res = cellRangeRef.Split(':');
            Start = new CellRef(res[0]);
            End = new CellRef(res[1]);
            FillRowColInfo();
        }

        public CellRangeRef(int startRow, int startColumn, int endRow, int endColumn)
        {
            Start = new CellRef(startRow, startColumn);
            End = new CellRef(endRow, endColumn);
            FillRowColInfo();
        }

        public CellRangeRef(CellRef start, CellRef end)
        {
            Start = start;
            End = end;
            FillRowColInfo();
        }

        private void FillRowColInfo()
        {
            TopRow = Math.Min(Start.Row, End.Row);
            BottomRow = Math.Max(Start.Row, End.Row);
            LeftColumn = Math.Min(Start.Column, End.Column);
            RightColumn = Math.Max(Start.Column, End.Column);
            RowCount = Math.Abs(Start.Row - End.Row) + 1;
            ColumnCount = Math.Abs(Start.Column - End.Column) + 1;
        }
    }
}
