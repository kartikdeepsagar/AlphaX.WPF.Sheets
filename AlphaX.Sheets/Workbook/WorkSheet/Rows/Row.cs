using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    internal class Row : IRow
    {
        private int _height;
        private string _styleName;
        private Rows _parent;

        public IFormatter Formatter { get; set; }

        public int Height
        {
            get
            {
                if (_height < 0)
                {
                    switch (_parent.Region)
                    {
                        case SheetRegion.ColumnHeader:
                            return _parent.ColumnHeaders.DefaultRowHeight;

                        default:
                            return _parent.WorkSheet.DefaultRowHeight;
                    }
                }

                return _height;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Row height can't be negative.");

                double oldHeight = Height;
                if (_parent.Region == SheetRegion.Cells)
                {
                    ((Rows)_parent.WorkSheet.Rows).UpdateRowsLocation(Index + 1, value - Height);
                }

                _height = value;

                _parent.WorkSheet.OnRowsChanged(new RowChangedEventArgs(
                    _parent.Region,
                    Index,
                    1, RowChangeType.Height));
            }
        }

        public IRows Parent => _parent;

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
                    _parent.WorkSheet.OnRowsChanged(new RowChangedEventArgs(
                        _parent.Region,
                        Index,
                        1, RowChangeType.Style));
                }

                _styleName = value;
            }
        }

        public bool Visible => Height > 0;
        internal int Index { get; set; }

        internal Row(Rows parent)
        {
            _parent = parent;
            _height = -1;
        }

        public void Dispose()
        {
            Formatter = null;
            StyleName = null;
            _parent = null;
        }
    }
}
