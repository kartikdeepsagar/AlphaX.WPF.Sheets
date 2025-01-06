using System;
using System.Collections.Generic;

namespace AlphaX.Sheets.Filtering
{
    public class FilterProvider : IFilterProvider
    {
        private WorkSheet _workSheet;
        private Dictionary<int, object> _filteredRows;

        internal IReadOnlyDictionary<int, object> FilteredRows => _filteredRows;

        public CellRange FilterRange { get; private set; }
        public FilterBase CurrentFilter { get; private set; }

        internal FilterProvider(WorkSheet workSheet)
        {
            _workSheet = workSheet;
            _filteredRows = new Dictionary<int, object>();
        }

        public void ApplyFilter(CellRange range, FilterBase filter)
        {
            CurrentFilter = filter;
            FilterRange = range;
            for(int col = range.LeftColumn; col <= range.RightColumn; col++)
            {
                for(int row = range.TopRow ; row <= range.BottomRow; row++)
                {
                    if (_filteredRows.ContainsKey(row))
                        continue;

                    var value = _workSheet.DataStore.GetValue(row, col);
                    if(!filter.PassesFilter(value))
                    {
                        _filteredRows.Add(row, null);
                    }
                }
            }
        }

        public void Clear()
        {
            _filteredRows.Clear();
        }
    }
}
