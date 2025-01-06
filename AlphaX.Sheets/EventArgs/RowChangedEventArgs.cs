using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaX.Sheets
{
    public class RowChangedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Index of the first row.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Afftected row count.
        /// </summary>
        public int Count { get; internal set; }
    }
}
