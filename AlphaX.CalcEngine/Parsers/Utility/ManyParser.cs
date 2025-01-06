using System.Collections.Generic;

namespace AlphaX.CalcEngine.Parsers
{
    internal class ManyParser : Parser
    {
        private Parser _parser;
        private int _minCount;

        public ManyParser(Parser parser, int minCount = 0)
        {
            _parser = parser;
            _minCount = minCount;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            List<ParserResult> results = new List<ParserResult>();
            var nextState = state;

            while (!nextState.IsError)
            {
                nextState = this._parser.Parse(nextState);
                if (!nextState.IsError)
                {
                    results.Add(nextState.Result);
                    state = nextState;
                }
            }

            if (results.Count < _minCount)
            {
                return UpdateError(state, new ParserError($"expected {_minCount} counts, but got {results.Count} counts"));
            }
            else
            {
                return UpdateResult(state, new ArrayResult(results.ToArray()));
            }
        }

    }

    internal class ManyOneParser : ManyParser
    {
        public ManyOneParser(Parser parser): base(parser, 1)
        {

        }
    }

    internal class ManySeptParser : Parser
    {
        private Parser Parser { get; set; }
        private Parser SeptByParser { get; set; }

        private int MinCount;

        public ManySeptParser(Parser parser, Parser septBy, int minCount = -1)
        {
            Parser = parser;
            SeptByParser = septBy;
            MinCount = minCount;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            List<ParserResult> results = new List<ParserResult>();
            var nextState = state;

            while (!nextState.IsError)
            {
                nextState = this.Parser.Parse(nextState);
                if (!nextState.IsError)
                {
                    results.Add(nextState.Result);
                    state = nextState;
                }

                nextState = this.SeptByParser.Parse(nextState);
            }

            if (results.Count < MinCount)
            {
                return UpdateError(state, new ParserError($"expected {MinCount} counts, but got {results.Count} counts"));
            }
            else
            {
                return UpdateResult(state, new ArrayResult(results.ToArray()));
            }

        }
    }

    internal class ManyMaxParser : Parser
    {
        private Parser _parser;
        private int _minCount;
        private int _maxCount;

        public ManyMaxParser(Parser parser, int minCount = 0, int maxCount = 1)
        {
            _parser = parser;
            _minCount = minCount;
            _maxCount = maxCount;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            List<ParserResult> results = new List<ParserResult>();
            var nextState = state;

            while (!nextState.IsError)
            {
                nextState = this._parser.Parse(nextState);
                if (!nextState.IsError)
                {
                    results.Add(nextState.Result);
                    state = nextState;
                }
            }

            if (results.Count < _minCount)
            {
                return UpdateError(state, new ParserError($"expected min {_minCount} counts, but got {results.Count} counts"));
            }
            else if (results.Count > _maxCount)
            {
                return UpdateError(state, new ParserError($"expected max {_minCount} counts, but got {results.Count} counts"));
            }
            else
            {
                return UpdateResult(state, new ArrayResult(results.ToArray()));
            }
        }

    }

    internal class ManyOccuranceParser : Parser
    {
        private Parser Parser { get; set; }

        public ManyOccuranceParser(Parser parser)
        {
            Parser = parser;
        }

        public override ParserState Parse(ParserState state)
        {
            if (state.IsError)
            {
                return state;
            }

            List<ParserResult> results = new List<ParserResult>();
            

            while (state.Index < state.InputString.Length)
            {
                var nextState = this.Parser.Parse(state);
                if (nextState.IsError)
                {
                    state = state.Clone();
                    state.Index++;
                }else
                {
                    state = nextState;
                    results.Add(nextState.Result);
                }

            }

            return UpdateResult(state, new ArrayResult(results.ToArray()));

        }
    }
}
