using System;

namespace AlphaX.CalcEngine.Parsers
{
    public class CalcParserResult : ParserResult
    {
        public CalcParserResultKind Kind { get; }
        public string Value { get; }

        private CalcParserResult[] _childs = null;
        public CalcParserResult[] Childs => _childs;

        public object ComputedValue { get; } = null;

        public CalcParserResult(CalcParserResultKind kind, string value, CalcParserResult[] childs = null)
        {
            Kind = kind;
            Value = value;
            this._childs = childs;

            switch (kind)
            {
                case CalcParserResultKind.String:
                    ComputedValue = value; break;
                case CalcParserResultKind.Number:
                    ComputedValue = int.Parse(value); break;
                case CalcParserResultKind.Float:
                    ComputedValue = double.Parse(value); break;
                case CalcParserResultKind.CellRef:
                    ComputedValue = new CellRef(value); break;
                case CalcParserResultKind.CellRangeRef:
                    ComputedValue = new CellRangeRef(value); break;
                case CalcParserResultKind.Operator:
                    ComputedValue = GetCalcOperatorFromString(value); break;
                case CalcParserResultKind.Bool:
                    ComputedValue = value == "true"? true: false; break;
            }
        }

        private CalcOperators GetCalcOperatorFromString(string opStr)
        {
            var op = CalcOperators.Unknown;
            switch (opStr)
            {
                case "+": op = CalcOperators.Plus; break;
                case "-": op = CalcOperators.Minus; break;
                case "*": op = CalcOperators.Multiply; break;
                case "/": op = CalcOperators.Divide; break;
                case "&": op = CalcOperators.ConcateString; break;
            }
            return op;
        }

        public void AddChild(CalcParserResult child)
        {
            if (_childs == null)
            {
                _childs = new CalcParserResult[0];
            }
            Array.Resize(ref _childs, _childs.Length + 1);
            _childs[_childs.Length - 1] = child;
        }
    }
}
