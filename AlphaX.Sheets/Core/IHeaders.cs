using System;

namespace AlphaX.Sheets
{
    public interface IHeaders : IDisposable
    {
        /// <summary>
        /// Gets the worksheet this headers belongs to.
        /// </summary>
        IWorkSheet WorkSheet { get; }    
        /// <summary>
        /// Gets header row collection.
        /// </summary>
        IRows Rows { get; }
        /// <summary>
        /// Gets header column collection.
        /// </summary>
        IColumns Columns { get; }
        /// <summary>
        /// Gets header cells.
        /// </summary>
        IRange Cells { get; }
    }
}
