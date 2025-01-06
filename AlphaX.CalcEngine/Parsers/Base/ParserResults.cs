namespace AlphaX.CalcEngine.Parsers
{
    public class ParserResult { }

    class StringResult : ParserResult
    {
        public string Value { get; set; } = "";

        public StringResult(string value)
        {
            Value = value;
        }
    }

    class ArrayResult : ParserResult
    {
        public ParserResult[] Value { get; set; } = new ParserResult[] { };

        public ArrayResult(ParserResult[] value)
        {
            Value = value;
        }
    }
}