using AlphaX.Parserz;
using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers.TokenParsers
{
    public class CellRefTokenParser : RegexParser<StringResult>
    {
        public CellRefTokenParser() 
            : base(new Regex(@"^([A-Za-z0-9_ ]+!)?[a-zA-Z]+[0-9]+", RegexOptions.Compiled), true) 
        { 
        }

        protected override StringResult ConvertResult(Match value)
        {
            return new StringResult(value.Value);
        }

        protected override IParserError CreateError(int index, string value)
        {
            return new ParserError(index, "Invalid cell reference token.");
        }
    }
}
