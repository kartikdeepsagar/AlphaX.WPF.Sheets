using System.Collections.Generic;

namespace AlphaX.CalcEngine.Parsers
{
    internal class CalcCellMetaInfo
    {
        public string Formula { get; set; }
        public CalcValue CalculatedValue { get; set; }
        public CalcParserResult CalcChain { get; set; }
        public IList<object> Dependencies { get; set; }
        public ISet<CellRef> Dependents { get; set; }

    }
}
