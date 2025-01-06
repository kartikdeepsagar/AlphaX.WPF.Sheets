using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers
{
    internal class LettersParser : RegexParser
    {
        static string _lettersRegex = "^[a-zA-Z]+";

        public LettersParser() : base(new Regex(_lettersRegex))
        {

        }

        protected override ParserError InternalErrorMap(ParserState state, ParserError error)
        {
            return new ParserError($"Error at index {state.Index}, Expected letters");
        }
    }
}
