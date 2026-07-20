using System.Collections.Generic;

namespace AlphaX.CalcEngine.Parsers
{
    internal class CalcCellMetaInfo
    {
        public string Formula { get; set; }
        public object CalculatedValue { get; set; }
        public ISet<CellRef> Dependents { get; set; }
        public IList<object> Dependencies { get; set; }
    }
}
