using System.Collections.Generic;

namespace AlphaX.CalcEngine.Parsers
{
    internal class SequenceOfParser : Parser
    {
        private Parser[] _parsers;

        public SequenceOfParser(Parser[] parsers)
        {
            this._parsers = parsers;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            List<ParserResult> results = new List<ParserResult>();
            var nextState = state;

            foreach (var p in _parsers)
            {
                nextState = p.Parse(nextState);
                if (!nextState.IsError)
                {
                    results.Add(nextState.Result);
                }
                else
                {
                    return this.UpdateError(state, nextState.Error);
                }
            }

            return UpdateResult(nextState, new ArrayResult(results.ToArray()));
        }
    }
}
