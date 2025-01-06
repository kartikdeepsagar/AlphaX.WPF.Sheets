namespace AlphaX.Sheets.Filtering
{
    public interface IFilterProvider
    {
        CellRange FilterRange { get; }
        FilterBase CurrentFilter { get; }
        /// <summary>
        /// Applies filter on the specified cell range.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="filter"></param>
        void ApplyFilter(CellRange range, FilterBase filter);
        /// <summary>
        /// Clears the applied filter.
        /// </summary>
        void Clear();
    }
}
