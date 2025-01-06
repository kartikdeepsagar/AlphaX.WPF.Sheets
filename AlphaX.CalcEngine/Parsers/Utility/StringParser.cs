namespace AlphaX.CalcEngine.Parsers
{
    internal class StringParser : Parser
    {
        public string Value { get; set; }

        public StringParser(string value)
        {
            Value = value;
        }

        public override ParserState Parse(ParserState state)
        {

            if (state.IsError)
            {
                return state;
            }

            var str = state.InputString.Substring(state.Index);

            if(str.Length < Value.Length)
            {
                return UpdateError(state, new ParserError($"Unexpected end of input, expected ${Value}, found end of input"));
            }

            if (str.StartsWith(Value))
            {
                return UpdateState(state, state.Index + Value.Length, new StringResult(Value));
            }

            
            return UpdateError(state, new ParserError ($"No match found, expected {Value} but got {str}" ));
            
        }
    }
}
