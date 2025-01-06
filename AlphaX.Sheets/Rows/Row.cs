using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    public class Row : IRow
    {
        private int _height;
        private string _styleName;

        public IFormatter Formatter { get; set; }

        public int Height
        {
            get
            {
                if (_height < 0)
                {
                    if (Parent.Parent is IWorkSheet workSheet)
                    {
                        return workSheet.DefaultRowHeight;
                    }
                    else if (Parent.Parent is IRowHeaders rowHeaders)
                    {
                        return rowHeaders.WorkSheet.DefaultRowHeight;
                    }
                    else if (Parent.Parent is IColumnHeaders columnHeaders)
                    {
                        return columnHeaders.DefaultRowHeight;
                    }

                    return 0;
                }

                return _height;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Row height can't be negative.");

                double oldHeight = Height;
                if (Parent.Parent is WorkSheet workSheet)
                {
                    workSheet.Rows.UpdateRowsLocation(Index + 1, value - Height);
                }

                _height = value;

                if (Parent.Parent is WorkSheet sheet)
                {
                    sheet?.OnRowsChanged(new RowChangedEventArgs()
                    { 
                        Index = Index,
                        NewValue = value,
                        OldValue = oldHeight,
                        Count = 1,
                        ChangeType = ChangeType.Size, 
                        Action = SheetAction.None
                    });
                }
            }
        }

        public Rows Parent { get; private set; }

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
                        worksheet.OnRowsChanged(new RowChangedEventArgs()
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

        public bool Visible => Height > 0;
        internal int Index { get; set; }

        internal Row(Rows parent)
        {
            Parent = parent;
            _height = -1;
        }

        public void Dispose()
        {
            Formatter = null;
            StyleName = null;
            Parent = null;
        }
    }
}
