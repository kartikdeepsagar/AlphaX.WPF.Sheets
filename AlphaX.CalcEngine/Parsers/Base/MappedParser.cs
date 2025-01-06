namespace AlphaX.CalcEngine.Parsers
{
    internal delegate ParserResult MapDelegate(ParserResult input);
    internal delegate ParserError ErrorMapDelegate(ParserError input);

    internal class MappedParser : Parser
    {

        private Parser Parser { get; set; }
        private MapDelegate MapFn { get; set; }
        private ErrorMapDelegate ErrorMapFn { get; set; }
        public MappedParser(Parser parser, MapDelegate mapFn = null, ErrorMapDelegate errorMapFn = null)
        {
            Parser = parser;
            MapFn = mapFn;
            ErrorMapFn = errorMapFn;
        }

        public override ParserState Parse(ParserState state)
        {
            var newState = Parser.Parse(state);
            if (newState.IsError)
            {
                return UpdateError(newState, newState.Error);
            }

            return UpdateResult(newState, newState.Result);

        }

        protected override ParserResult InternalResultMap(ParserState state, int index, ParserResult result)
        {
            return MapFn == null ? base.InternalResultMap(state, index, result) : MapFn(result);
        }

        protected override ParserError InternalErrorMap(ParserState state, ParserError error)
        {
            return ErrorMapFn == null ? base.InternalErrorMap(state, error) : ErrorMapFn(error);
        }
    }
}
