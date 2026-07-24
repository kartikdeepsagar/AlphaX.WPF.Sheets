using AlphaX.CalcEngine;
using AlphaX.Sheets.Core;
using System;

namespace AlphaX.Sheets
{
    public interface IWorkBook : IDisposable, IStyleProvider
    {
        /// <summary>
        /// Gets or sets the name for this workbook.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets the sheet collection of this workbook.
        /// </summary>
        IWorkSheets WorkSheets { get; }
        /// <summary>
        /// Gets the calculation engine of this worksheet.
        /// </summary>
        ICalcEngine CalcEngine { get; }
        /// <summary>
        /// Gets the style palette of this workbook.
        /// </summary>
        IStylePalette StylePalette { get; }

        /// <summary>
        /// Gets the values of the range as 2d array
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        object[,] GetRangeValue(string sheetName, int rowIndex, int columnIndex, int rowCount, int columnCount);
    }
}