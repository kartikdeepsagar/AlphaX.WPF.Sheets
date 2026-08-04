using System;
using System.Collections.Generic;

namespace DevBrewLabs.Spreadsheet.Sorting
{
    /// <summary>
    /// Encapsulates the configuration for a sorting operation.
    /// </summary>
    public class SortOptions
    {
        public List<SortInfo> SortLevels { get; set; } = new List<SortInfo>();
        public bool MatchCase { get; set; } = false;
        public bool HasHeader { get; set; } = false;
        
        /// <summary>
        /// If true, only the columns specified in SortLevels are reordered.
        /// Otherwise, entire rows in the specified range are reordered.
        /// </summary>
        public bool SortColumnOnly { get; set; } = false;
    }
}
