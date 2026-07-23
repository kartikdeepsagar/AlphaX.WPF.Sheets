using AlphaX.Sheets.Core;
using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;

namespace AlphaX.Sheets
{
    public class Cell : IRange
    {
        private object _value;
        private string _styleName;
        private IStyle _style;
        private IFormatter _formatter;
        private DataMap _dataMap;
        private ICellType _cellType;
        private bool _locked;
        private int _rowSpan = 1;
        private int _columnSpan = 1;

        private ColumnData ColData => (Parent?.SheetParent is WorkSheet ws) ? Parent.GetColumnData(Column, true) : null;
        private ColumnData ColDataReadOnly => (Parent?.SheetParent is WorkSheet ws) ? Parent.GetColumnData(Column, false) : null;
        private StylePalette Palette => (Parent?.SheetParent is WorkSheet ws && ws.WorkBook is WorkBook wb) ? wb.StylePalette : null;

        public IFormatter Formatter
        {
            get
            {
                if (ColDataReadOnly != null)
                    return ColDataReadOnly.GetFormatter(Row);

                return _formatter;
            }
            set
            {
                if (ColData != null)
                    ColData.SetFormatter(Row, value);

                _formatter = value;
            }
        }

        public object Value
        {
            get
            {
                if(Parent?.SheetParent is WorkSheet worksheet)
                {
                    return worksheet.DataStore.GetValue(Row, Column);
                }

                return _value;
            }
            set
            {
                if (Parent?.SheetParent is WorkSheet worksheet)
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
                if (Parent?.SheetParent is WorkSheet worksheet)
                {
                    return worksheet.WorkBook.CalcEngine.GetFormula(worksheet.Name, Row, Column);
                }

                return null;
            }
            set
            {
                if (Parent?.SheetParent is WorkSheet worksheet)
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
                if (ColDataReadOnly != null)
                    return ColDataReadOnly.GetStyleName(Row);

                return _styleName;
            }
            set
            {
                var oldStyleName = StyleName;
                if(oldStyleName != value)
                {
                    if (ColData != null)
                        ColData.SetStyleName(Row, value);

                    if (Parent?.SheetParent is WorkSheet worksheet)
                    {
                        worksheet.OnCellChanged(new CellChangedEventArgs()
                        {
                            Row = Row,
                            OldValue = oldStyleName,
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
                if (ColDataReadOnly != null && Palette != null)
                {
                    ushort styleId = ColDataReadOnly.GetStyleId(Row);
                    if (styleId != StylePalette.DefaultStyleId)
                        return Palette.GetStyle(styleId);
                }

                return _style;
            }
            set
            {
                var oldStyle = Style;
                if (oldStyle != value)
                {
                    if (ColData != null && Palette != null)
                    {
                        ushort styleId = Palette.GetOrAdd(value);
                        ColData.SetStyleId(Row, styleId);
                    }

                    if (Parent?.SheetParent is WorkSheet worksheet)
                    {
                        worksheet.OnCellChanged(new CellChangedEventArgs()
                        {
                            Row = Row,
                            OldValue = oldStyle,
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
        public DataMap DataMap
        {
            get
            {
                if (ColDataReadOnly != null)
                    return ColDataReadOnly.GetDataMap(Row) as DataMap;

                return _dataMap;
            }
            set
            {
                if (ColData != null)
                    ColData.SetDataMap(Row, value);

                _dataMap = value;
            }
        }

        public ICellType CellType
        {
            get
            {
                if (ColDataReadOnly != null)
                    return ColDataReadOnly.GetCellType(Row);

                return _cellType;
            }
            set
            {
                if (ColData != null)
                    ColData.SetCellType(Row, value);

                _cellType = value;
            }
        }

        internal object MetaData { get; set; }
        internal int Row { get; set; }
        internal int Column { get; set; }
        public bool HasFormula => !string.IsNullOrEmpty(Formula);

        public bool Locked
        {
            get
            {
                if (ColDataReadOnly != null)
                    return ColDataReadOnly.GetLocked(Row);

                return _locked;
            }
            set
            {
                if (ColData != null)
                    ColData.SetLocked(Row, value);

                _locked = value;
            }
        }

        public bool IsVisible { get; set; }

        public int RowSpan
        {
            get
            {
                if (ColDataReadOnly != null)
                {
                    int span = ColDataReadOnly.GetRowSpan(Row);
                    return span == 0 ? 1 : span;
                }

                return _rowSpan;
            }
            set
            {
                if (ColData != null)
                    ColData.SetRowSpan(Row, value);

                _rowSpan = value;
            }
        }

        public int ColumnSpan
        {
            get
            {
                if (ColDataReadOnly != null)
                {
                    int span = ColDataReadOnly.GetColumnSpan(Row);
                    return span == 0 ? 1 : span;
                }

                return _columnSpan;
            }
            set
            {
                if (ColData != null)
                    ColData.SetColumnSpan(Row, value);

                _columnSpan = value;
            }
        }

        public int RowCount => 1;
        public int ColumnCount => 1;

        public IRange this[int row, int column, int rowCount, int columnCount] => this;
        public IRange this[int row, int column] => this;
        public IRange this[string name] => this;

        internal Cell(Cells parent)
        {
            Parent = parent;
            IsVisible = true;
            _rowSpan = 1;
            _columnSpan = 1;
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
