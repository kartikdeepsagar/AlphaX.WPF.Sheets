namespace AlphaX.CalcEngine.Parsers
{
    internal class BetweenParser : SequenceOfParser
    {
        public BetweenParser(Parser left, Parser right, Parser content) : base(new Parser[]{ left, content, right})
        {

        }

        public override ParserState Parse(ParserState state)
        {
            var nextState = base.Parse(state);

            if (nextState.IsError)
            {
                return nextState;
            }
            else
            {
                return this.UpdateResult(nextState, (nextState.Result as ArrayResult).Value[1]);
            }
        }
    }
}
