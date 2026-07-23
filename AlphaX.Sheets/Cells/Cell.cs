using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;

namespace AlphaX.Sheets
{
    public class Cell : IRange
    {
        private object _value;
        private string _styleName;
        private IStyle _style;

        public IFormatter Formatter { get; set; }
        public object Value
        {
            get
            {
                if(Parent.Parent is WorkSheet worksheet)
                {
                    return worksheet.DataStore.GetValue(Row, Column);
                }

                return _value;
            }
            set
            {
                if (Parent.Parent is WorkSheet worksheet)
                {
                    if (HasFormula && value != null)
                        Formula = null;

                    var oldValue = Value;
                    worksheet.DataStore.SetValue(Row, Column, value);

                    worksheet.OnCellChanged(new CellChangedEventArgs()
                    {
                        Row = Row,
                        OldValue = oldValue,
                        NewValue = value,
                        Column = Column,
                        ChangeType = ChangeType.Value,
                        Action = SheetAction.None
                    });
                    return;
                }

                _value = value;
            }
        }
        public string Formula
        {
            get
            {
                if (Parent.Parent is WorkSheet worksheet)
                {
                    return worksheet.WorkBook.CalcEngine.GetFormula(worksheet.Name, Row, Column);
                }

                return null;
            }
            set
            {
                if (Parent.Parent is WorkSheet worksheet)
                {
                    if (value != null && Value != null)
                        Value = null;

                    var oldValue = Formula;
                    worksheet.WorkBook.CalcEngine.SetFormula(worksheet.Name, Row, Column, value);

                    worksheet.OnCellChanged(new CellChangedEventArgs()
                    {
                        Row = Row,
                        OldValue = oldValue,
                        NewValue = Value,
                        Column = Column,
                        ChangeType = ChangeType.Formula,
                        Action = SheetAction.None
                    });
                }
            }
        }
        public string StyleName
        {
            get
            {
                return _styleName;
            }
            set
            {
                if(_styleName != value)
                {
                    if (Parent.Parent is WorkSheet worksheet)
                    {
                        worksheet.OnCellChanged(new CellChangedEventArgs()
                        {
                            Row = Row,
                            OldValue = _styleName,
                            NewValue = value,
                            Column = Column,
                            ChangeType = ChangeType.Style,
                            Action = SheetAction.None
                        });
                    }
                }

                _styleName = value;
            }
        }

        public IStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                if (_style != value)
                {
                    if (Parent.Parent is WorkSheet worksheet)
                    {
                        worksheet.OnCellChanged(new CellChangedEventArgs()
                        {
                            Row = Row,
                            OldValue = _style,
                            NewValue = value,
                            Column = Column,
                            ChangeType = ChangeType.Style,
                            Action = SheetAction.None
                        });
                    }
                }

                _style = value;
            }
        }

        public Cells Parent { get; private set; }
        public DataMap DataMap { get; set; }
        public ICellType CellType { get; set; }

        internal object MetaData { get; set; }
        internal int Row { get; set; }
        internal int Column { get; set; }
        public bool HasFormula => !string.IsNullOrEmpty(Formula);
        public bool Locked { get; set; }
        public bool IsVisible { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public int RowCount => 1;
        public int ColumnCount => 1;

        public IRange this[int row, int column, int rowCount, int columnCount] => this;
        public IRange this[int row, int column] => this;
        public IRange this[string name] => this;

        internal Cell(Cells parent)
        {
            Parent = parent;
            IsVisible = true;
            RowSpan = 1;
            ColumnSpan = 1;
        }

        public void Dispose()
        {
            Value = null;
            Formula = null;
            Formatter = null;
            MetaData = null;
            DataMap = null;
            Parent = null;
            CellType = null;
            StyleName = null;
            Style = null;
        }
    }
}
