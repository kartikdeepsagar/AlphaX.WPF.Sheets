using AlphaX.Sheets.Formatters;
using System.Collections.Generic;

namespace AlphaX.Sheets.Core
{
    /// <summary>
    /// Holds a snapshot of raw cell data for a single row within a column.
    /// </summary>
    internal struct CellData
    {
        public object Value;
        public string Formula;
        public ushort StyleId;
        public string StyleName;
        public IFormatter Formatter;
        public ICellType CellType;
        public IDataMap DataMap;
        public int RowSpan;
        public int ColumnSpan;
        public bool Locked;
        public bool IsEmpty => Value == null && Formula == null && StyleId == 0 && StyleName == null && Formatter == null && CellType == null && DataMap == null && RowSpan == 0 && ColumnSpan == 0 && !Locked;
    }

    /// <summary>
    /// Columnar data storage for a single column across all rows.
    /// </summary>
    public class ColumnData
    {
        public int ColumnIndex { get; }

        private readonly Dictionary<int, object> _values;
        private readonly Dictionary<int, string> _formulas;
        private readonly Dictionary<int, ushort> _styleIds;
        private readonly Dictionary<int, string> _styleNames;
        private readonly Dictionary<int, IFormatter> _formatters;
        private readonly Dictionary<int, ICellType> _cellTypes;
        private readonly Dictionary<int, IDataMap> _dataMaps;
        private readonly Dictionary<int, int> _rowSpans;
        private readonly Dictionary<int, int> _columnSpans;
        private readonly Dictionary<int, bool> _locked;

        public ColumnData(int columnIndex)
        {
            ColumnIndex = columnIndex;
            _values = new Dictionary<int, object>();
            _formulas = new Dictionary<int, string>();
            _styleIds = new Dictionary<int, ushort>();
            _styleNames = new Dictionary<int, string>();
            _formatters = new Dictionary<int, IFormatter>();
            _cellTypes = new Dictionary<int, ICellType>();
            _dataMaps = new Dictionary<int, IDataMap>();
            _rowSpans = new Dictionary<int, int>();
            _columnSpans = new Dictionary<int, int>();
            _locked = new Dictionary<int, bool>();
        }

        #region Values
        public object GetValue(int row)
        {
            _values.TryGetValue(row, out var val);
            return val;
        }

        public void SetValue(int row, object value)
        {
            if (value == null)
                _values.Remove(row);
            else
                _values[row] = value;
        }
        #endregion

        #region Formulas
        public string GetFormula(int row)
        {
            _formulas.TryGetValue(row, out var val);
            return val;
        }

        public void SetFormula(int row, string formula)
        {
            if (string.IsNullOrEmpty(formula))
                _formulas.Remove(row);
            else
                _formulas[row] = formula;
        }
        #endregion

        #region Styles
        public ushort GetStyleId(int row)
        {
            _styleIds.TryGetValue(row, out var val);
            return val;
        }

        public void SetStyleId(int row, ushort styleId)
        {
            if (styleId == StylePalette.DefaultStyleId)
                _styleIds.Remove(row);
            else
                _styleIds[row] = styleId;
        }

        public string GetStyleName(int row)
        {
            _styleNames.TryGetValue(row, out var val);
            return val;
        }

        public void SetStyleName(int row, string styleName)
        {
            if (string.IsNullOrEmpty(styleName))
                _styleNames.Remove(row);
            else
                _styleNames[row] = styleName;
        }
        #endregion

        #region Formatters
        public IFormatter GetFormatter(int row)
        {
            _formatters.TryGetValue(row, out var val);
            return val;
        }

        public void SetFormatter(int row, IFormatter formatter)
        {
            if (formatter == null)
                _formatters.Remove(row);
            else
                _formatters[row] = formatter;
        }
        #endregion

        #region CellTypes
        public ICellType GetCellType(int row)
        {
            _cellTypes.TryGetValue(row, out var val);
            return val;
        }

        public void SetCellType(int row, ICellType cellType)
        {
            if (cellType == null)
                _cellTypes.Remove(row);
            else
                _cellTypes[row] = cellType;
        }
        #endregion

        #region DataMaps
        public IDataMap GetDataMap(int row)
        {
            _dataMaps.TryGetValue(row, out var val);
            return val;
        }

        public void SetDataMap(int row, IDataMap dataMap)
        {
            if (dataMap == null)
                _dataMaps.Remove(row);
            else
                _dataMaps[row] = dataMap;
        }
        #endregion

        #region Spans & Locked
        public int GetRowSpan(int row)
        {
            _rowSpans.TryGetValue(row, out var val);
            return val;
        }

        public void SetRowSpan(int row, int span)
        {
            if (span == 0)
                _rowSpans.Remove(row);
            else
                _rowSpans[row] = span;
        }

        public int GetColumnSpan(int row)
        {
            _columnSpans.TryGetValue(row, out var val);
            return val;
        }

        public void SetColumnSpan(int row, int span)
        {
            if (span == 0)
                _columnSpans.Remove(row);
            else
                _columnSpans[row] = span;
        }

        public bool GetLocked(int row)
        {
            _locked.TryGetValue(row, out var val);
            return val;
        }

        public void SetLocked(int row, bool locked)
        {
            if (!locked)
                _locked.Remove(row);
            else
                _locked[row] = locked;
        }
        #endregion

        #region CellData Operations
        internal CellData GetCellData(int row)
        {
            return new CellData
            {
                Value = GetValue(row),
                Formula = GetFormula(row),
                StyleId = GetStyleId(row),
                StyleName = GetStyleName(row),
                Formatter = GetFormatter(row),
                CellType = GetCellType(row),
                DataMap = GetDataMap(row),
                RowSpan = GetRowSpan(row),
                ColumnSpan = GetColumnSpan(row),
                Locked = GetLocked(row)
            };
        }

        internal void SetCellData(int row, CellData data)
        {
            SetValue(row, data.Value);
            SetFormula(row, data.Formula);
            SetStyleId(row, data.StyleId);
            SetStyleName(row, data.StyleName);
            SetFormatter(row, data.Formatter);
            SetCellType(row, data.CellType);
            SetDataMap(row, data.DataMap);
            SetRowSpan(row, data.RowSpan);
            SetColumnSpan(row, data.ColumnSpan);
            SetLocked(row, data.Locked);
        }

        internal void ClearRow(int row)
        {
            _values.Remove(row);
            _formulas.Remove(row);
            _styleIds.Remove(row);
            _styleNames.Remove(row);
            _formatters.Remove(row);
            _cellTypes.Remove(row);
            _dataMaps.Remove(row);
            _rowSpans.Remove(row);
            _columnSpans.Remove(row);
            _locked.Remove(row);
        }

        public bool HasRowData(int row)
        {
            return _values.ContainsKey(row) || _formulas.ContainsKey(row) || _styleIds.ContainsKey(row) ||
                   _styleNames.ContainsKey(row) || _formatters.ContainsKey(row) || _cellTypes.ContainsKey(row) ||
                   _dataMaps.ContainsKey(row) || _rowSpans.ContainsKey(row) || _columnSpans.ContainsKey(row) ||
                   _locked.ContainsKey(row);
        }

        public void Clear()
        {
            _values.Clear();
            _formulas.Clear();
            _styleIds.Clear();
            _styleNames.Clear();
            _formatters.Clear();
            _cellTypes.Clear();
            _dataMaps.Clear();
            _rowSpans.Clear();
            _columnSpans.Clear();
            _locked.Clear();
        }
        #endregion
    }
}
