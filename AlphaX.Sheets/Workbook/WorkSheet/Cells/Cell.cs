using AlphaX.Sheets.Core;
using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    internal class Cell : IRange
    {
        private object _value;
        private WorkSheet _workSheet;
        private IFormatter _formatter;
        private IStyle _style;
        private int _rowSpan = 1;
        private int _columnSpan = 1;
        private Cells _parentRange;

        private ColumnData ColData => _parentRange.GetColumnData(Column, true);
        private ColumnData ColDataReadOnly => _parentRange.GetColumnData(Column, false);
        private IStylePalette Palette => _workSheet.WorkBook.StylePalette;

        public IFormatter Formatter
        {
            get
            {
                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        if(ColDataReadOnly == null)
                        {
                            return null;
                        }
                        return ColDataReadOnly.GetFormatter(Row);

                    default:
                        return _formatter;
                }
            }
            set
            {
                var oldValue = Formatter;

                if (oldValue == value)
                {
                    return;
                }

                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        ColData.SetFormatter(Row, value);
                        break;

                    default:
                        _formatter = value;
                        break;
                }
            }
        }

        public object Value
        {
            get
            {
                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        return _workSheet.DataStore.GetValue(Row, Column);

                    default:
                        return _value;
                }
            }
            set
            {
                var oldValue = Value;

                if (oldValue == value)
                {
                    return;
                }

                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        if (HasFormula && value != null)
                            Formula = null;

                        _workSheet.DataStore.SetValue(Row, Column, value);
                        break;

                    default:
                        _value = value;
                        break;
                }

                _workSheet.OnCellChanged(new CellChangedEventArgs(
                       _parentRange.Region,
                       Row,
                       Column,
                       oldValue,
                       value,
                       CellChangeType.Value));
            }
        }

        public string Formula
        {
            get
            {
                if (_parentRange.Region != SheetRegion.Cells)
                {
                    return null;
                }

                return _workSheet.WorkBook.CalcEngine.GetFormula(_workSheet.Name, Row, Column);
            }
            set
            {
                if (_parentRange.Region != SheetRegion.Cells)
                {
                    throw new InvalidOperationException(string.Format("Formula not allowed in {0}", _parentRange.Region));
                }

                var oldValue = Formula;

                if (oldValue == value)
                {
                    return;
                }

                if (value != null && Value != null)
                    Value = null;
                _workSheet.WorkBook.CalcEngine.SetFormula(_workSheet.Name, Row, Column, value);

                _workSheet.OnCellChanged(new CellChangedEventArgs(
                      _parentRange.Region,
                      Row,
                      Column,
                      oldValue,
                      value,
                      CellChangeType.Formula));
            }
        }

        public string StyleName
        {
            get
            {
                if (ColDataReadOnly != null)
                    return ColDataReadOnly.GetStyleName(Row);

                return null;
            }
            set
            {
                var oldStyleName = StyleName;

                if(oldStyleName != value)
                {
                    ColData.SetStyleName(Row, value);
                    _workSheet.OnCellChanged(new CellChangedEventArgs(
                        _parentRange.Region, 
                        Row, 
                        Column, 
                        oldStyleName, 
                        value, 
                        CellChangeType.Style));
                }
            }
        }

        public IStyle Style
        {
            get
            {
                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        if (ColDataReadOnly == null || Palette == null)
                        {
                            return null;
                        }
                        ushort styleId = ColDataReadOnly.GetStyleId(Row);
                        if (styleId != StylePalette.DefaultStyleId)
                            return Palette.GetStyle(styleId);
                        return null;

                    default:
                        return _style;
                }
            }
            set
            {
                var oldStyle = Style;

                if (oldStyle == value)
                {
                    return;
                }

                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        ushort styleId = Palette.GetOrAdd(value);
                        ColData.SetStyleId(Row, styleId);
                        break;

                    default:
                        _style = value;
                        break;
                }

                _workSheet.OnCellChanged(new CellChangedEventArgs(
                       _parentRange.Region,
                       Row,
                       Column,
                       oldStyle,
                       value,
                       CellChangeType.Style));
            }
        }

        public DataMap DataMap
        {
            get
            {
                if (_parentRange.Region != SheetRegion.Cells || ColDataReadOnly == null)
                {
                    return null;
                }

                return ColDataReadOnly.GetDataMap(Row) as DataMap;
            }
            set
            {
                if (_parentRange.Region != SheetRegion.Cells)
                {
                    throw new InvalidOperationException(string.Format("DataMap not allowed in {0}", _parentRange.Region));
                }

                ColData.SetDataMap(Row, value);
            }
        }

        public ICellType CellType
        {
            get
            {
                if (_parentRange.Region != SheetRegion.Cells || ColDataReadOnly == null)
                {
                    return null;
                }

                return ColDataReadOnly.GetCellType(Row); ;
            }
            set
            {
                if (_parentRange.Region != SheetRegion.Cells)
                {
                    throw new InvalidOperationException(string.Format("CellType not allowed in {0}", _parentRange.Region));
                }

                ColData.SetCellType(Row, value);
            }
        }

        public bool Locked
        {
            get
            {
                if (_parentRange.Region != SheetRegion.Cells || ColDataReadOnly == null)
                {
                    return false;
                }

                return ColDataReadOnly.GetLocked(Row);
            }
            set
            {
                if (_parentRange.Region != SheetRegion.Cells)
                {
                    throw new InvalidOperationException(string.Format("Locked not allowed in {0}", _parentRange.Region));
                }

                ColData.SetLocked(Row, value);
            }
        }

        public int RowSpan
        {
            get
            {
                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        if (ColDataReadOnly == null)
                        {
                            return 0;
                        }
                        return ColDataReadOnly.GetRowSpan(Row);

                    default:
                        return _rowSpan;
                }
            }
            set
            {
                var oldValue = RowSpan;

                if (oldValue == value)
                {
                    return;
                }

                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        ColData.SetRowSpan(Row, value);
                        break;

                    default:
                        _rowSpan = value;
                        break;
                }
            }
        }

        public int ColumnSpan
        {
            get
            {
                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        if (ColDataReadOnly == null)
                        {
                            return 0;
                        }
                        return ColDataReadOnly.GetColumnSpan(Row);

                    default:
                        return _columnSpan;
                }
            }
            set
            {
                var oldValue = ColumnSpan;

                if (oldValue == value)
                {
                    return;
                }

                switch (_parentRange.Region)
                {
                    case SheetRegion.Cells:
                        ColData.SetColumnSpan(Row, value);
                        break;

                    default:
                        _columnSpan = value;
                        break;
                }
            }
        }

        public int RowCount => 1;
        public int ColumnCount => 1;
        internal object MetaData { get; set; }
        internal int Row { get; set; }
        internal int Column { get; set; }
        public bool HasFormula => !string.IsNullOrEmpty(Formula);
        public IRange ParentRange => _parentRange;
        public bool IsVisible { get; internal set; }

        public IRange this[int row, int column, int rowCount, int columnCount] => this;
        public IRange this[int row, int column] => this;
        public IRange this[string name] => this;

        internal Cell(Cells parent)
        {
            _workSheet = parent.GetWorkSheet();
            _parentRange = parent;
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
            _parentRange = null;
            CellType = null;
            StyleName = null;
            Style = null;
        }
    }
}
