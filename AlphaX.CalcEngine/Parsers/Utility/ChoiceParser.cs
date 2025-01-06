using System;

namespace AlphaX.CalcEngine.Parsers
{
    internal class ChoiceParser : Parser
    {
        private Parser[] _parsers;
        private Lazy<Parser[]> _lazyParsers;
        private int _minCount = -1;

        public ChoiceParser(Parser[] parsers, int minCount = -1)
        {
            _parsers = parsers;
            _minCount = minCount;
        }

        // lazy version
        public ChoiceParser(Lazy<Parser[]> lazyParserFactory)
        {
            _lazyParsers = lazyParserFactory;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            var parsers = _parsers;
            if(_lazyParsers != null)
            {
                parsers = _lazyParsers.Value;
            }

            foreach (var p in parsers)
            {
                var nextState = p.Parse(state);
                if (!nextState.IsError)
                {
                    return nextState;
                }
            }

            return UpdateError(state, new ParserError($"Unable to match with any parser at index { state.Index}"));
        }
    }
}
