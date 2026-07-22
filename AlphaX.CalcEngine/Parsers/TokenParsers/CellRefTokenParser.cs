using AlphaX.Parserz;
using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers.TokenParsers
{
    internal class CellRefTokenParser : RegexParser<StringResult>
    {
        private static readonly Regex RefRegex = new Regex(@"^([A-Za-z0-9_ ]+!)?[a-zA-Z]+[0-9]+", RegexOptions.Compiled);

        public CellRefTokenParser() 
            : base(RefRegex, true) 
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
