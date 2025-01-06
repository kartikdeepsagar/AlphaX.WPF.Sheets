using AlphaX.CalcEngine;
using AlphaX.Sheets;
using AlphaX.WPF.Sheets.CellTypes;
using AlphaX.WPF.Sheets.UI.Editors;
using AlphaX.WPF.Sheets.UI.Interaction;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal class EditingManager : UIManager, IEditingManager
    {
        public EditingManager(AlphaXSpread spread) : base(spread)
        {
            UseCellValue = true;
        }

        public FrameworkElement ActiveEditor { get; private set; }
        public bool IsEditing => ActiveEditor != null;
        internal bool UseCellValue { get; set; }

        public void BeginEdit(int row, int column)
        {
            if (IsEditing)
                return;

            var sheetView = Spread.SheetViews.ActiveSheetView;
            var workSheet = sheetView.WorkSheet;

            var sheetColumn = workSheet.Columns.GetItem(column, false);

            if (sheetColumn != null && sheetColumn.Locked)
                return;

            var cellsInteractionLayer = sheetView.Spread.SheetViewPane.CellsRegion.GetInteractionLayer();
            var cellRect = sheetView.ViewPort.GetCellRect(row, column);
            cellRect.X -= sheetView.ViewPort.As<ViewPort>().LeftColumnLocation;
            cellRect.Y -= sheetView.ViewPort.As<ViewPort>().TopRowLocation;
            var cell = workSheet.Cells.GetCell(row, column, false);

            if (cell != null && cell.Locked)
                return;

            var sheetRow = workSheet.Rows.GetItem(row, false);
            var cellType = RenderingExtensions.GetCellType(cell, sheetColumn);

            var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow);
            if (style == null)
                style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultSheetStyleKey);

            var editor = cellType.GetEditor(style.As<Style>());
            editor.SheetView = sheetView;
            ActiveEditor = editor;

            if (cell != null && !string.IsNullOrEmpty(cell.Formula))
            {
                editor.Text = $"={cell.Formula}";
            }
            else
            {
                var value = workSheet.As<WorkSheet>().DataStore.GetValue(row, column);
                var formatter = workSheet.PickFormatter(cell, sheetColumn, sheetRow);
                editor.Text = formatter.Format(value);
            }

            if (!UseCellValue)
                editor.Text = "";

            editor.Row = row;
            editor.Column = column;
            editor.KeyDown += OnEditorKeyDown;
            editor.CaretIndex = editor.Text.Length;
            editor.MinWidth = cellRect.Width - 2;
            editor.Height = cellRect.Height - 2;
            cellsInteractionLayer.Children.Add(ActiveEditor);
            Canvas.SetLeft(ActiveEditor, cellRect.X + 1);
            Canvas.SetTop(ActiveEditor, cellRect.Y + 1);
            editor.Focus();
        }

        private void OnEditorKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    EndEdit(false);
                    break;
            }
        }

        public bool EndEdit(bool commitChanges)
        {
            if (!IsEditing)
                return false;

            var sheetView = Spread.SheetViews.ActiveSheetView;
            var cellsInteractionLayer = sheetView.Spread.SheetViewPane.CellsRegion.GetInteractionLayer();

            if (!commitChanges)
            {
                cellsInteractionLayer.Children.Remove(ActiveEditor);
                ActiveEditor = null;
                return true;
            }

            if (ActiveEditor is AlphaXTextBox gcTextBox)
            {
                return EndTextCellEdit(gcTextBox, sheetView, cellsInteractionLayer);
            }
            else if(ActiveEditor is AlphaXNumericEditor numTextBox)
            {
                return EndNumericCellEdit(numTextBox, sheetView, cellsInteractionLayer);
            }

            return false;
        }

        private bool EndNumericCellEdit(AlphaXNumericEditor numTextBox, IAlphaXSheetView sheetView, InteractionLayer layer)
        {
            var workSheet = sheetView.WorkSheet;
            var cellChangedAction = new CellChangedAction() { SheetView = sheetView.As<AlphaXSheetView>() };
            cellChangedAction.OldState.Value = workSheet.DataStore.GetValue(numTextBox.Row, numTextBox.Column);
            cellChangedAction.OldState.Row = numTextBox.Row;
            cellChangedAction.OldState.Column = numTextBox.Column;
            cellChangedAction.OldState.Selection = sheetView.Selection.Clone();

            var value = DataTypeConverter.ConvertType(numTextBox.Text);
            workSheet.Cells[numTextBox.Row, numTextBox.Column].Value = value;

            cellChangedAction.NewState.Value = value;
            cellChangedAction.NewState.Row = numTextBox.Row;
            cellChangedAction.NewState.Column = numTextBox.Column;
            cellChangedAction.NewState.Selection = sheetView.Selection.Clone();

            Spread.UndoRedoManager.AddAction(cellChangedAction);

            layer.Children.Remove(ActiveEditor);
            ActiveEditor.KeyDown -= OnEditorKeyDown;
            ActiveEditor = null;
            layer.Focus();
            return true;
        }

        private bool EndTextCellEdit(AlphaXTextBox gcTextBox, IAlphaXSheetView sheetView, InteractionLayer layer)
        {
            var workSheet = sheetView.WorkSheet;
            if (gcTextBox.Text.StartsWith("="))
            {
                try
                {
                    workSheet.Cells[gcTextBox.Row, gcTextBox.Column].Formula = gcTextBox.Text.Substring(1);
                }
                catch (CalcEngineException ex)
                {
                    sheetView.Spread.RaiseCalculationError(new CalcErrorEventArgs()
                    {
                        Exception = ex,
                        Row = gcTextBox.Row,
                        Column = gcTextBox.Column,
                        Formula = gcTextBox.Text,
                        SheetView = sheetView
                    });
                    ActiveEditor.Focus();
                    return false;
                }
            }
            else
            {
                var cellChangedAction = new CellChangedAction() { SheetView = sheetView.As<AlphaXSheetView>() };
                cellChangedAction.OldState.Value = workSheet.DataStore.GetValue(gcTextBox.Row, gcTextBox.Column);
                cellChangedAction.OldState.Row = gcTextBox.Row;
                cellChangedAction.OldState.Column = gcTextBox.Column;
                cellChangedAction.OldState.Selection = sheetView.Selection.Clone();

                var value = DataTypeConverter.ConvertType(gcTextBox.Text);
                workSheet.Cells[gcTextBox.Row, gcTextBox.Column].Value = value;

                cellChangedAction.NewState.Value = value;
                cellChangedAction.NewState.Row = gcTextBox.Row;
                cellChangedAction.NewState.Column = gcTextBox.Column;
                cellChangedAction.NewState.Selection = sheetView.Selection.Clone();

                Spread.UndoRedoManager.AddAction(cellChangedAction);
            }

            layer.Children.Remove(ActiveEditor);
            ActiveEditor.KeyDown -= OnEditorKeyDown;
            ActiveEditor = null;
            layer.Focus();
            return true;
        }
    }
}
