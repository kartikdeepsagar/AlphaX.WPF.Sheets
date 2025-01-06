using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers
{
    internal class RegexParser : Parser
    {
        private Regex _regex;

        public RegexParser(Regex regex)
        {
            _regex = regex;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            string inp = state.InputString.Substring(state.Index);
            var match = _regex.Match(inp);

            if (match.Success)
            {
                return UpdateState(state, state.Index + match.Value.Length, new StringResult(match.Value));
            }

            return UpdateError(state, new ParserError($"No match found, expected pattern {_regex}, found {state.InputString}"));
        }

    }
}
