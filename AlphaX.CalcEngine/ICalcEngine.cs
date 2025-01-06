using AlphaX.CalcEngine.Formulas;
using System.Collections.Generic;

namespace AlphaX.CalcEngine
{
    public interface ICalcEngine
    {
        event CellRecalculatedEventHandler CellRecalculated;
        CalcValue EvaluateExpression(string str, string defaultName);
        object GetValue(string sheetName, int row, int column);
        void SetFormula(string sheetName, int row, int column, string formula);
        string GetFormula(string sheetName, int row, int column);
        void RegisterCustomFormula(Formula formula);
        List<object> GetRangesInFormulaString(string formula);
        IEnumerable<Formula> GetRegisteredFormulas();
    }
}
