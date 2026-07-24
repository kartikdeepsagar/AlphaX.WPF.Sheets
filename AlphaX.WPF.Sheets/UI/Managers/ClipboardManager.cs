using AlphaX.Sheets;
using AlphaX.WPF.Sheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    /// <summary>
    /// Enterprise-grade clipboard manager handling copy, paste, Win32 clipboard locking retries,
    /// internal data objects, TSV/CSV format parsing, formula insertion, and undo/redo integration.
    /// </summary>
    public class ClipboardManager : IClipboardManager
    {
        private readonly AlphaXSpread _spread;
        private const int MaxClipboardRetries = 5;
        private const int ClipboardRetryDelayMs = 30;

        public ClipboardManager(AlphaXSpread spread)
        {
            _spread = spread ?? throw new ArgumentNullException(nameof(spread));
        }

        #region Public Methods

        public void Copy()
        {
            var activeSheetView = _spread.SheetViews?.ActiveSheetView;
            if (activeSheetView != null)
            {
                Copy(activeSheetView, activeSheetView.Selection);
            }
        }

        public void Copy(IAlphaXSheetView sheetView)
        {
            if (sheetView == null)
                throw new ArgumentNullException(nameof(sheetView));

            Copy(sheetView, sheetView.Selection);
        }

        public void Copy(IAlphaXSheetView sheetView, CellRange range)
        {
            if (sheetView == null || range == null || sheetView.WorkSheet == null)
                return;

            if (range.RowCount <= 0 || range.ColumnCount <= 0)
                return;

            var stringBuilder = new StringBuilder();
            var data = sheetView.WorkSheet.GetData(
                range.TopRow,
                range.LeftColumn,
                range.RowCount,
                range.ColumnCount);

            for (int row = 0; row < data.GetLength(0); row++)
            {
                for (int column = 0; column < data.GetLength(1); column++)
                {
                    var val = data[row, column];
                    var strVal = val != null ? val.ToString() : string.Empty;
                    stringBuilder.Append(FormatTsvCell(strVal));

                    if (column < range.ColumnCount - 1)
                        stringBuilder.Append(SheetUtils.Tab);
                }

                if (row < range.RowCount - 1)
                    stringBuilder.Append(SheetUtils.NextLine);
            }

            var dataObject = new DataObject();
            var textContent = stringBuilder.ToString();
            dataObject.SetData(DataFormats.UnicodeText, textContent);
            dataObject.SetData(DataFormats.Text, textContent);
            dataObject.SetData("InternalDataObject", data);

            ExecuteWithRetry(() => Clipboard.SetDataObject(dataObject));
        }

        public void Paste()
        {
            var activeSheetView = _spread.SheetViews?.ActiveSheetView;
            if (activeSheetView != null)
            {
                Paste(activeSheetView);
            }
        }

        public void Paste(IAlphaXSheetView sheetView)
        {
            if (sheetView == null || sheetView.WorkSheet == null)
                return;

            var dataObject = ExecuteWithRetry(() => Clipboard.GetDataObject());
            if (dataObject == null)
                return;

            object[,] data = ExtractClipboardData(dataObject);
            if (data == null)
                return;

            var concreteSheetView = sheetView as AlphaXSheetView;
            if (concreteSheetView == null)
                return;

            int activeRow = concreteSheetView.ActiveRow;
            int activeColumn = concreteSheetView.ActiveColumn;
            var workSheet = concreteSheetView.WorkSheet;

            _spread.SuspendUpdates = true;

            try
            {
                var pasteAction = new ClipboardPasteAction() { SheetView = concreteSheetView };
                pasteAction.OldState.Value = concreteSheetView.WorkSheet.GetData(activeRow, activeColumn, data.GetLength(0), data.GetLength(1));
                pasteAction.OldState.Row = activeRow;
                pasteAction.OldState.Column = activeColumn;
                pasteAction.OldState.Selection = concreteSheetView.Selection.Clone();

                for (int row = 0; row < data.GetLength(0); row++)
                {
                    for (int column = 0; column < data.GetLength(1); column++)
                    {
                        var value = data[row, column];
                        if (value is string strVal && strVal.StartsWith("=") && strVal.Length > 1)
                        {
                            try
                            {
                                workSheet.Cells[activeRow + row, activeColumn + column].Formula = strVal.Substring(1);
                            }
                            catch
                            {
                                workSheet.Cells[activeRow + row, activeColumn + column].Value = value;
                            }
                        }
                        else
                        {
                            workSheet.Cells[activeRow + row, activeColumn + column].Value = value;
                        }
                    }
                }

                _spread.SelectionManager.SelectRange(activeRow, activeColumn, data.GetLength(0), data.GetLength(1));

                pasteAction.NewState.Value = data;
                pasteAction.NewState.Row = activeRow;
                pasteAction.NewState.Column = activeColumn;
                pasteAction.NewState.Selection = concreteSheetView.Selection.Clone();

                _spread.UndoRedoManager.AddAction(pasteAction);
            }
            finally
            {
                _spread.SuspendUpdates = false;
            }
        }

        public bool CanCopy(IAlphaXSheetView sheetView = null)
        {
            var targetView = sheetView ?? _spread.SheetViews?.ActiveSheetView;
            if (targetView == null || targetView.Selection == null)
                return false;

            return targetView.Selection.RowCount > 0 && targetView.Selection.ColumnCount > 0;
        }

        public bool CanPaste(IAlphaXSheetView sheetView = null)
        {
            var targetView = sheetView ?? _spread.SheetViews?.ActiveSheetView;
            if (targetView == null)
                return false;

            var dataObject = ExecuteWithRetry(() => Clipboard.GetDataObject());
            if (dataObject == null)
                return false;

            return dataObject.GetDataPresent("InternalDataObject") ||
                   dataObject.GetDataPresent(DataFormats.UnicodeText) ||
                   dataObject.GetDataPresent(DataFormats.Text) ||
                   dataObject.GetDataPresent(DataFormats.StringFormat) ||
                   dataObject.GetDataPresent(DataFormats.CommaSeparatedValue);
        }

        #endregion

        #region Private Helper Methods

        private object[,] ExtractClipboardData(IDataObject dataObject)
        {
            if (dataObject.GetDataPresent("InternalDataObject"))
            {
                var internalData = dataObject.GetData("InternalDataObject") as object[,];
                if (internalData != null)
                    return internalData;
            }

            string text = null;
            if (dataObject.GetDataPresent(DataFormats.UnicodeText))
                text = dataObject.GetData(DataFormats.UnicodeText) as string;
            else if (dataObject.GetDataPresent(DataFormats.Text))
                text = dataObject.GetData(DataFormats.Text) as string;
            else if (dataObject.GetDataPresent(DataFormats.StringFormat))
                text = dataObject.GetData(DataFormats.StringFormat) as string;

            if (!string.IsNullOrEmpty(text))
            {
                return ParseTextData(text, '\t');
            }

            if (dataObject.GetDataPresent(DataFormats.CommaSeparatedValue))
            {
                var csvObj = dataObject.GetData(DataFormats.CommaSeparatedValue);
                if (csvObj is string csvText)
                    return ParseTextData(csvText, ',');
                if (csvObj is Stream csvStream)
                {
                    using (var reader = new StreamReader(csvStream, Encoding.UTF8))
                    {
                        return ParseTextData(reader.ReadToEnd(), ',');
                    }
                }
            }

            return null;
        }

        internal static object[,] ParseTextData(string text, char delimiter)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            int lineCount = lines.Length;

            if (lineCount > 1 && string.IsNullOrEmpty(lines[lineCount - 1]) && (text.EndsWith("\n") || text.EndsWith("\r")))
            {
                lineCount--;
            }

            if (lineCount == 0)
                return null;

            var parsedRows = new List<string[]>();
            int maxColumns = 0;

            for (int i = 0; i < lineCount; i++)
            {
                var rowCells = lines[i].Split(delimiter);
                var cleanCells = new string[rowCells.Length];
                for (int j = 0; j < rowCells.Length; j++)
                {
                    var cell = rowCells[j];
                    if (cell.Length >= 2 && cell.StartsWith("\"") && cell.EndsWith("\""))
                    {
                        cell = cell.Substring(1, cell.Length - 2).Replace("\"\"", "\"");
                    }
                    cleanCells[j] = cell;
                }

                if (cleanCells.Length > maxColumns)
                    maxColumns = cleanCells.Length;

                parsedRows.Add(cleanCells);
            }

            if (maxColumns == 0)
                return null;

            var data = new object[parsedRows.Count, maxColumns];
            for (int r = 0; r < parsedRows.Count; r++)
            {
                var rowCells = parsedRows[r];
                for (int c = 0; c < maxColumns; c++)
                {
                    if (c < rowCells.Length)
                    {
                        var val = rowCells[c];
                        data[r, c] = DataTypeConverter.ConvertType(val);
                    }
                    else
                    {
                        data[r, c] = null;
                    }
                }
            }

            return data;
        }

        private static string FormatTsvCell(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.Contains("\t") || text.Contains("\n") || text.Contains("\r") || text.Contains("\""))
            {
                return "\"" + text.Replace("\"", "\"\"") + "\"";
            }

            return text;
        }

        private static T ExecuteWithRetry<T>(Func<T> action)
        {
            for (int i = 0; i < MaxClipboardRetries; i++)
            {
                try
                {
                    return action();
                }
                catch (COMException) when (i < MaxClipboardRetries - 1)
                {
                    Thread.Sleep(ClipboardRetryDelayMs);
                }
                catch (ExternalException) when (i < MaxClipboardRetries - 1)
                {
                    Thread.Sleep(ClipboardRetryDelayMs);
                }
            }
            return default(T);
        }

        private static void ExecuteWithRetry(Action action)
        {
            for (int i = 0; i < MaxClipboardRetries; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (COMException) when (i < MaxClipboardRetries - 1)
                {
                    Thread.Sleep(ClipboardRetryDelayMs);
                }
                catch (ExternalException) when (i < MaxClipboardRetries - 1)
                {
                    Thread.Sleep(ClipboardRetryDelayMs);
                }
            }
        }

        #endregion
    }
}
