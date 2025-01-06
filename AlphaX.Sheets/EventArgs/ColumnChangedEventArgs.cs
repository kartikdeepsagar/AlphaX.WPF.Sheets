using System.Collections.Generic;
using System.Text;

namespace AlphaX.Sheets
{
    public class ColumnChangedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Index of the first column.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Afftected column count.
        /// </summary>
        public int Count { get; internal set; }
    }
}
