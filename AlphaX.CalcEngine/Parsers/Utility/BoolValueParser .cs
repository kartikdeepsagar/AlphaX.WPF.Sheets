using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers
{
    internal class BoolValueParser : Parser
    {
        private Regex _regex;

        public BoolValueParser()
        {
            _regex = new Regex(ParserRegexes.GetVarParserRegex());
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
                var val = match.Value.ToLowerInvariant();
                if(val == "true" || val == "false")
                {
                    return UpdateState(state, state.Index + match.Value.Length, new StringResult(val));
                }
            }

            return UpdateError(state, new ParserError($"No match found, expected bool value, found {state.InputString}"));
        }

    }
}
