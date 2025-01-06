using AlphaX.CalcEngine;
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
        WorkSheets WorkSheets { get; }
        /// <summary>
        /// Gets the calculation engine of this worksheet.
        /// </summary>
        ICalcEngine CalcEngine { get; }
        /// <summary>
        /// Gets the update provider.
        /// </summary>
        IUpdateProvider UpdateProvider { get; }
    }
}