using System;

namespace AlphaX.WPF.Sheets
{
    internal class CellChangedAction : SheetAction
    {
        public State OldState { get; private set; }
        public State NewState { get; private set; }
        public AlphaXSheetView SheetView { get; set; }

        public CellChangedAction()
        {
            OldState = new State();
            NewState = new State();
        }

        public override void Undo()
        {
            Execute(OldState);
        }

        public override void Redo()
        {
            Execute(NewState);
        }

        private void Execute(State state)
        {
            SheetView.WorkSheet.Cells[state.Row, state.Column].Value = state.Value;
            var selection = state.Selection;
            SheetView.ActiveRow = state.Row;
            SheetView.ActiveColumn = state.Column;
            SheetView.Spread.SelectionManager.SelectRange(selection.TopRow, selection.LeftColumn, selection.RowCount, selection.ColumnCount);
            SheetView.Invalidate();
        }
    }
}
