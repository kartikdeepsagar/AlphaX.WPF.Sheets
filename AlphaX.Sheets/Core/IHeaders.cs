using System;

namespace AlphaX.Sheets
{
    public interface IHeaders : IDisposable
    {
        /// <summary>
        /// Gets the worksheet this headers belongs to.
        /// </summary>
        WorkSheet WorkSheet { get; }    
        /// <summary>
        /// Gets header row collection.
        /// </summary>
        Rows Rows { get; }
        /// <summary>
        /// Gets header column collection.
        /// </summary>
        Columns Columns { get; }
        /// <summary>
        /// Gets header cells.
        /// </summary>
        Cells Cells { get; }
    }
}
