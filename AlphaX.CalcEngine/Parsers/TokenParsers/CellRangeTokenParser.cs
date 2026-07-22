using AlphaX.Parserz;
using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers.TokenParsers
{
    internal class CellRangeTokenParser : RegexParser<StringResult>
    {
        private static readonly Regex RangeRegex = new Regex(@"^([A-Za-z0-9_ ]+!)?[a-zA-Z]+[0-9]+:[a-zA-Z]+[0-9]+", RegexOptions.Compiled);

        public CellRangeTokenParser() 
            : base(RangeRegex, true) 
        { 
        }

        protected override StringResult ConvertResult(Match value)
        {
            return new StringResult(value.Value);
        }

        protected override IParserError CreateError(int index, string value)
        {
            return new ParserError(index, "Invalid cell range token.");
        }
    }
}
