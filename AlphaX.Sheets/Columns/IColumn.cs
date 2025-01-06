using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    public interface IColumn : IDisposable, IStyledObject
    {
        /// <summary>
        /// Gets the parent column collection.
        /// </summary>
        Columns Parent { get; }
        /// <summary>
        /// Gets or sets the width of this column.
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Gets or sets whether the column supports editing.
        /// </summary>
        bool Locked { get; set; }
        /// <summary>
        /// Gets or sets the data map for this column.
        /// </summary>
        DataMap DataMap { get; set; }
        /// <summary>
        /// Gets or sets the cell type for this column.
        /// </summary>
        ICellType CellType { get; set; }
        /// <summary>
        /// Gets whether the column is visible.
        /// </summary>
        bool Visible { get; }
        /// <summary>
        /// Gets or sets the column formatter.
        /// </summary>
        IFormatter Formatter { get; set; }
    }
}
