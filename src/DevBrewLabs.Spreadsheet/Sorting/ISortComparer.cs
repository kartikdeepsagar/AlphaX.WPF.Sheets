using System;

namespace DevBrewLabs.Spreadsheet.Sorting
{
    /// <summary>
    /// Interface for custom sorting logic in worksheets.
    /// </summary>
    public interface ISortComparer
    {
        /// <summary>
        /// Compares two cell values and returns an integer that indicates whether the first instance precedes, follows, or occurs in the same position in the sort order as the second instance.
        /// </summary>
        int Compare(object x, object y);
    }
}
