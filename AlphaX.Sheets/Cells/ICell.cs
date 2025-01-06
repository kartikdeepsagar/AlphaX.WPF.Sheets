using AlphaX.Sheets.Data;
using AlphaX.Sheets.Formatters;
using System;

namespace AlphaX.Sheets
{
    public interface ICell : IDisposable, IStyledObject
    {
        /// <summary>
        /// Gets the parent cell collection.
        /// </summary>
        Cells Parent { get; }
        /// <summary>
        /// Gets or sets the cell value.
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// Gets or sets the cell formula.
        /// </summary>
        string Formula { get; set; }
        /// <summary>
        /// Gets whether the cell has formula.
        /// </summary>
        bool HasFormula { get; }
        /// <summary>
        /// Gets or sets the cell type.
        /// </summary>
        ICellType CellType { get; set; }
        /// <summary>
        /// Gets or sets the data map.
        /// </summary>
        DataMap DataMap { get; set; }
        /// <summary>
        /// Gets or sets whether the cell is locked.
        /// </summary>
        bool Locked { get; set; }
        /// <summary>
        /// Gets or sets the cell formatter.
        /// </summary>
        IFormatter Formatter { get; set; }
        /// <summary>
        /// Gets or sets whether this cell is visible or not.
        /// </summary>
        bool IsVisible { get; set; }
        /// <summary>
        /// Gets or sets the row span for this cell.
        /// </summary>
        int RowSpan { get; set; }
        /// <summary>
        /// Gets or sets the column span for this cell.
        /// </summary>
        int ColumnSpan { get; set; }
    }
}
