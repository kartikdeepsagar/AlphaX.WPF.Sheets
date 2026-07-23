using System;

namespace AlphaX.Sheets
{
    public interface IColumns : IEnumerableEx<IColumn>, IDisposable
    {
        /// <summary>
        /// Gets the parent this collection belongs to.
        /// </summary>
        object Parent { get; }
        /// <summary>
        /// Gets the column present at the provided index.
        /// </summary>
        /// <param name="index">
        /// Column index. 
        /// </param>
        /// <returns></returns>
        IColumn this[int index] { get; }
        /// <summary>
        /// Gets the column with specific column name.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        IColumn this[string address] { get; }
        /// <summary>
        /// Gets the column width.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        int GetColumnWidth(int column);

        /// <summary>
        /// Gets the column index of the provided column.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        int GetColumnIndex(IColumn column);
    }
}
