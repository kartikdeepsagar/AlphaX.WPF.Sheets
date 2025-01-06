using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers
{
    internal class DigitParser : RegexParser
    {
        static string _digitsRegex = "^-?[0-9]+";

        public DigitParser(): base(new Regex(_digitsRegex))
        {
            
        }

        protected override ParserError InternalErrorMap(ParserState state, ParserError error)
        {
            return new ParserError($"Error at index {state.Index}, Expected digits");
        }
    }
}
