using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    internal class Column : IColumn
    {
        private int _width;
        private DataMap _dataMap;
        private Columns _parent;
        private string _styleName;

        public IFormatter Formatter { get; set; }

        public int Width
        {
            get
            {
                if (_width < 0)
                {
                    switch (_parent.Region)
                    {
                        case SheetRegion.RowHeader:
                            return _parent.RowHeaders.DefaultColumnWidth;

                        default:
                            return _parent.WorkSheet.DefaultColumnWidth;
                    }
                }

                return _width;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Column width can't be negative.");

                double oldWidth = Width;
                
                if (_parent.Region == SheetRegion.Cells)
                {
                    ((Columns)_parent.WorkSheet.Columns).UpdateColumnsLocation(Index + 1, value - Width);
                }

                _width = value;

                _parent.WorkSheet?.OnColumnsChanged(new ColumnChangedEventArgs(
                    _parent.Region,
                    Index,
                    1,
                    ColumnChangeType.Width));
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
                    _parent.WorkSheet?.OnColumnsChanged(new ColumnChangedEventArgs(
                       _parent.Region,
                       Index,
                        1,
                        ColumnChangeType.Style));
                }

                _styleName = value;
            }
        }

        public IColumns Parent => _parent;
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
            _parent = parent;
            _width = -1;
            Locked = false;
        }
        internal int Index { get; set; }

        private void OnDataMapChanged()
        {
            if(_parent.Region == SheetRegion.Cells)
            {
                ((Cells)_parent.WorkSheet.Cells).ClearColumnCells(Parent.GetColumnIndex(this));
            }
        }

        public void Dispose()
        {
            StyleName = null;
            CellType = null;
            DataMap = null;
            Formatter = null;
            _parent = null;
        }
    }
}
