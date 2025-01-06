namespace AlphaX.CalcEngine.Parsers
{
    internal class ParserState
    {
        public string InputString { get; set; }
        public int Index { get; set; }
        public ParserResult Result { get; set; }
        public bool IsError { get; set; } = false;
        public ParserError Error { get; set; }

        public ParserState()
        {
            Result = new ParserResult();
            InputString = string.Empty;
        }

        public ParserState Clone()
        {
            var state = this;

            var newState = new ParserState
            {
                InputString = state.InputString,
                Index = state.Index,
                Result = state.Result,
                IsError = state.IsError,
                Error = state.Error
            };
            return newState;
        }
    }
}
