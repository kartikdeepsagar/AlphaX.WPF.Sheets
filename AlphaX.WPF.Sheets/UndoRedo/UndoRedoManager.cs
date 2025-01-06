using System.Collections.Generic;

namespace AlphaX.WPF.Sheets
{
    public class UndoRedoManager
    {
        private Stack<SheetAction> _undoStack;
        private Stack<SheetAction> _redoStack;
        private AlphaXSpread _spread;

        public UndoRedoManager(AlphaXSpread spread)
        {
            _spread = spread;
            _undoStack = new Stack<SheetAction>();
            _redoStack = new Stack<SheetAction>();
        }

        public void AddAction(SheetAction action)
        {
            _undoStack.Push(action);

            if(_redoStack.Count > 0)
                _redoStack.Clear();
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var action = _redoStack.Pop();
                action.Redo();
                _undoStack.Push(action);
            }
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var action = _undoStack.Pop();
                action.Undo();
                _redoStack.Push(action);
            }
        }
    }
}
