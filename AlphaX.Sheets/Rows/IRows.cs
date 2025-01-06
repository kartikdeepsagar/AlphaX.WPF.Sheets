using System;

namespace AlphaX.Sheets
{
    public interface IRows : IEnumerableEx<Row>, IDisposable
    {
        /// <summary>
        /// Gets the parent this collection belongs to.
        /// </summary>
        object Parent { get; }
        /// <summary>
        /// Gets the row present at the provided index.
        /// </summary>
        /// <param name="index">
        /// Row index.
        /// </param>
        /// <returns></returns>
        Row this[int index] { get; }
        /// <summary>
        /// Gets the row height.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        int GetRowHeight(int row);
    }
}
