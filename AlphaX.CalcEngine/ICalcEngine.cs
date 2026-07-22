using AlphaX.FormulaEngine;
using System.Collections.Generic;

namespace AlphaX.CalcEngine
{
    /// <summary>
    /// Represents the core calculation engine for spreadsheet processing, providing methods for evaluating expressions, managing cell formulas, and handling dependencies.
    /// </summary>
    public interface ICalcEngine
    {
        /// <summary>
        /// Occurs when a cell's value is recalculated due to formula evaluation or dependency updates.
        /// </summary>
        event CellRecalculatedEventHandler CellRecalculated;

        /// <summary>
        /// Evaluates a standalone formula expression.
        /// </summary>
        /// <param name="expression">The formula expression to evaluate.</param>
        /// <param name="sheetName">The name of the sheet context in which to evaluate the expression.</param>
        /// <returns>The calculated <see cref="CalcValue"/>.</returns>
        CalcValue EvaluateExpression(string expression, string sheetName = "");

        /// <summary>
        /// Gets the computed value of a specific cell.
        /// </summary>
        /// <param name="sheetName">The name of the sheet.</param>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="column">The column index of the cell.</param>
        /// <returns>The computed value of the cell, or null if empty or uncalculated.</returns>
        object GetValue(string sheetName, int row, int column);

        /// <summary>
        /// Sets a formula for a specific cell, updating its dependencies and triggering recalculations.
        /// </summary>
        /// <param name="sheetName">The name of the sheet.</param>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="column">The column index of the cell.</param>
        /// <param name="formula">The formula string to set.</param>
        void SetFormula(string sheetName, int row, int column, string formula);

        /// <summary>
        /// Gets the raw formula string set in a specific cell.
        /// </summary>
        /// <param name="sheetName">The name of the sheet.</param>
        /// <param name="row">The row index of the cell.</param>
        /// <param name="column">The column index of the cell.</param>
        /// <returns>The formula string, or null if no formula is present.</returns>
        string GetFormula(string sheetName, int row, int column);

        /// <summary>
        /// Registers a custom formula to be available within the calculation engine.
        /// </summary>
        /// <param name="formula">The custom formula instance to register.</param>
        void RegisterCustomFormula(FormulaBase formula);

        /// <summary>
        /// Extracts all cell ranges and references present within a formula string.
        /// </summary>
        /// <param name="formula">The formula string to parse.</param>
        /// <returns>A list containing cell ranges and references found in the formula.</returns>
        List<object> GetRangesInFormulaString(string formula);

        /// <summary>
        /// Retrieves a collection of all formulas registered in the calculation engine.
        /// </summary>
        /// <returns>An enumerable collection of registered <see cref="FormulaInfo"/>.</returns>
        IEnumerable<FormulaInfo> GetRegisteredFormulas();
    }
}
