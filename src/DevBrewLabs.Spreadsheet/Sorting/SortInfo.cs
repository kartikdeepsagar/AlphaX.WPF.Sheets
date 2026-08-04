namespace DevBrewLabs.Spreadsheet.Sorting
{
    /// <summary>
    /// Represents a single level of sorting.
    /// </summary>
    public class SortInfo
    {
        public int ColumnIndex { get; set; }
        public bool Ascending { get; set; } = true;
        
        /// <summary>
        /// Optional custom comparer for this specific column. 
        /// If null, the default natural sorting is used.
        /// </summary>
        public ISortComparer CustomComparer { get; set; }
        
        public SortInfo(int columnIndex, bool ascending = true)
        {
            ColumnIndex = columnIndex;
            Ascending = ascending;
        }
    }
}
