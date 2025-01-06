using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    public interface IRow : IDisposable, IStyledObject
    {
        /// <summary>
        /// Gets the parent row collection.
        /// </summary>
        Rows Parent { get; }
        /// <summary>
        /// Gets or sets the height of this row.
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// Gets whether the row is visible.
        /// </summary>
        bool Visible { get; }
        /// <summary>
        /// Gets or sets the row formatter.
        /// </summary>
        IFormatter Formatter { get; set; }
    }
}
