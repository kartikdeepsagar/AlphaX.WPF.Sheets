namespace AlphaX.CalcEngine.Parsers
{
    internal delegate Parser ChainDelegate(ParserResult result);

    internal class ChainedParser : Parser
    {
        private Parser Parser { get; set; }
        private ChainDelegate ChainFn { get; set; }

        public ChainedParser(Parser parser, ChainDelegate chainFn)
        {
            Parser = parser;
            ChainFn = chainFn;
        }

        public override ParserState Parse(ParserState state)
        {
            var newState = Parser.Parse(state);
            if (newState.IsError)
            {
                return newState;
            }

            var newParser = ChainFn(newState.Result);
            return newParser.Parse(newState);
        }
    }
}
