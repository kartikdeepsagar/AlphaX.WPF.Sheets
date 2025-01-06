using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    public class Column : IColumn
    {
        private int _width;
        private DataMap _dataMap;
        private string _styleName;

        public IFormatter Formatter { get; set; }

        public int Width
        {
            get
            {
                if (_width < 0)
                {
                    if (Parent.Parent is IWorkSheet workSheet)
                    {
                        return workSheet.DefaultColumnWidth;
                    }
                    else if (Parent.Parent is IRowHeaders rowHeaders)
                    {
                        return rowHeaders.DefaultColumnWidth;
                    }
                    else if (Parent.Parent is IColumnHeaders columnHeaders)
                    {
                        return columnHeaders.WorkSheet.DefaultColumnWidth;
                    }

                    return 0;
                }

                return _width;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Column width can't be negative.");

                double oldWidth = Width;
                
                if (Parent.Parent is WorkSheet workSheet)
                {
                    workSheet.Columns.UpdateColumnsLocation(Index + 1, value - Width);
                }

                _width = value;

                if (Parent.Parent is WorkSheet sheet)
                {
                    sheet?.OnColumnsChanged(new ColumnChangedEventArgs()
                    {
                        Index = Index,
                        NewValue = value,
                        OldValue = oldWidth,
                        Count = 1,
                        ChangeType = ChangeType.Size,
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
                if (_styleName != value)
                {
                    if (Parent.Parent is WorkSheet worksheet)
                    {
                        worksheet.OnColumnsChanged(new ColumnChangedEventArgs()
                        {
                            Index = Index,
                            OldValue = _styleName,
                            NewValue = value,
                            Count = 1,
                            ChangeType = ChangeType.Style,
                            Action = SheetAction.None
                        });
                    }
                }

                _styleName = value;
            }
        }

        public Columns Parent { get; private set; }
        public DataMap DataMap
        {
            get
            {
                return _dataMap;
            }
            set
            {
                _dataMap = value;
                OnDataMapChanged();
            }
        }
        public ICellType CellType { get; set; }
        public bool Locked { get; set; }
        public bool Visible => Width > 0;
        internal Column(Columns parent)
        {
            Parent = parent;
            _width = -1;
            Locked = false;
        }
        internal int Index { get; set; }

        private void OnDataMapChanged()
        {
            if(Parent.Parent is WorkSheet worksheet)
            {
                worksheet.Cells.ClearColumnCells(Parent.GetColumnIndex(this));
            }
        }

        public void Dispose()
        {
            StyleName = null;
            CellType = null;
            DataMap = null;
            Formatter = null;
            Parent = null;
        }
    }
}
