using AlphaX.CalcEngine;
using System;

namespace AlphaX.Sheets
{
    public class WorkBookDataProvider : IDataProvider, IDisposable
    {
        private WorkBook _workBook;

        public event ValueChangedEventHandler ValueChanged;

        public WorkBookDataProvider(WorkBook workBook)
        {
            _workBook = workBook;
        }

        public object[,] GetRangeValue(string sheetName, int rowIndex, int columnIndex, int rowCount, int columnCount)
        {
            var worksheet = _workBook.WorkSheets.GetSheet(sheetName);

            var data = new object[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    data[row, col] = worksheet.DataStore.GetValue(rowIndex + row, columnIndex + col);
                }
            }

            return data;
        }

        public object GetValue(string sheetName, int rowIndex, int columnIndex)
        {
            var worksheet = _workBook.WorkSheets.GetSheet(sheetName);
            return worksheet.DataStore.GetValue(rowIndex, columnIndex);
        }

        public void SetMetaData(string sheetName, int row, int column, object data)
        {
            var cell = _workBook.WorkSheets.GetSheet(sheetName).Cells[row, column];
            cell.MetaData = data;
        }

        public object GetMetaData(string sheetName, int row, int column)
        {
            var cell = _workBook.WorkSheets.GetSheet(sheetName).Cells.GetCell(row, column, false);

            if (cell == null)
                return null;

            return cell.MetaData;
        }

        internal void RaiseValueChanged(ValueChangedEventArgs args)
        {
            ValueChanged?.Invoke(args);
        }

        public void Dispose()
        {
            _workBook = null;
        }
    }
}
