using System;
using System.Collections.Generic;

namespace AlphaX.Sheets
{
    public interface IWorkSheets : IEnumerable<IWorkSheet>, IDisposable
    {
        /// <summary>
        /// Fired when a sheet is removed.
        /// </summary>
        event EventHandler<SheetChangedEventArgs> SheetRemoved;
        /// <summary>
        /// Fired when a sheet is added.
        /// </summary>
        event EventHandler<SheetChangedEventArgs> SheetAdded;
        /// <summary>
        /// Fired when active sheet is changed.
        /// </summary>
        event EventHandler<SheetChangedEventArgs> ActiveSheetChanged;
        /// <summary>
        /// Gets the sheet count.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Gets the parent workbook.
        /// </summary>
        IWorkBook WorkBook { get; }
        /// <summary>
        /// Gets the sheet by name.
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IWorkSheet this[string sheetName] { get; }
        /// <summary>
        /// Gets the sheet by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IWorkSheet this[int index] { get; }
        /// <summary>
        /// Gets or sets the active sheet.
        /// </summary>
        IWorkSheet ActiveSheet { get; set; }
        /// <summary>
        /// Gets or sets the active sheet index.
        /// </summary>
        int ActiveSheetIndex { get; set; }
        /// <summary>
        /// Adds a new sheet to this collection.
        /// </summary>
        /// <param name="name">
        ///  Name of the sheet.
        /// </param>
        /// <returns></returns>
        IWorkSheet AddSheet(string name);
        /// <summary>
        /// Removes the sheet.
        /// </summary>
        /// <param name="name"></param>
        void RemoveSheet(string name);
        /// <summary>
        /// Removes the sheet at specific index.
        /// </summary>
        /// <param name="index"></param>
        void RemoveSheet(int index);
        /// <summary>
        /// Removed all sheets.
        /// </summary>
        void Clear();
        /// <summary>
        /// Gets the sheet by name.
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IWorkSheet GetSheet(string sheetName);
        /// <summary>
        /// Gets sheet by index.
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        IWorkSheet GetSheet(int index);
    }
}
