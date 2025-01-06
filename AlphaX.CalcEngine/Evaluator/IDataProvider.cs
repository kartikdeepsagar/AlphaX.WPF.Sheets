using System;

namespace AlphaX.CalcEngine
{
    public delegate void ValueChangedEventHandler(ValueChangedEventArgs args);
    public interface IDataProvider
    {
        event ValueChangedEventHandler ValueChanged;
        void SetMetaData(string sheetName, int row, int column, object data);
        object GetMetaData(string sheetName, int row, int column);
        object GetValue(string sheetName, int rowIndex, int columnIndex);
        object[,] GetRangeValue(string sheetName, int rowIndex, int columnIndex, int rowCount, int columnCount);
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string SheetName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}
