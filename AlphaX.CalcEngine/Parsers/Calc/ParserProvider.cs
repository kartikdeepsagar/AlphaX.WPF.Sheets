using AlphaX.CalcEngine.Resources;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlphaX.CalcEngine.Parsers
{
    internal static class ParserProvider
    {
        #region Parsers
        public static Parser LettersParser { get; private set; }
        public static Parser DigitsParser { get; private set; }
        public static Parser SpaceParser { get; private set; }
        public static Parser NumberParser { get; private set; }
        public static Parser FloatParser { get; private set; }
        public static Parser VarParser { get; private set; }
        public static Parser CellRefParser { get; private set; }
        public static Parser CellRangeParser { get; private set; }
        public static Parser SheetNameCellRefParser { get; private set; }
        public static Parser SheetNameCellRangeParser { get; private set; }
        public static Parser OperatorParser { get; private set; }
        public static Parser OpenParanParser { get; private set; }
        public static Parser CloseParanParser { get; private set; }
        public static Parser FormulaArgumentParser { get; private set; }
        public static Parser FormulaParser { get; private set; }
        public static Parser OperandParser { get; private set; }
        public static Parser EquationParser { get; private set; }
        public static Parser StringValueParser { get; private set; }
        public static Parser BoolValueParser { get; private set; }
        public static Parser RangeFinderParser { get; private set; }
        #endregion

        static ParserProvider()
        {
            CreateBasicParsers();
            CreateComplexParsers();
        }

        #region Parsers Creation
        private static void CreateBasicParsers()
        {
            LettersParser = new LettersParser();

            DigitsParser = new DigitParser();

            SpaceParser = CreateManyParser(CreateStringParser(ParserTokens.BlankSpace));

            StringValueParser = new StringValueParser()
                .Map(StringParserMap)
                .MapError(err => new ParserError(ExceptionMessages.StringExpected));

            BoolValueParser = new BoolValueParser()
                .Map(BoolParserMap)
                .MapError(err => new ParserError(ExceptionMessages.BoolExpected));

            NumberParser = DigitsParser
                .Map(NumberParserMap)
                .MapError(err => new ParserError(ExceptionMessages.NumberExpected));

            FloatParser = CreateRegexParser(ParserRegexes.GetFloatParserRegex())
                .Map(FloatParserMap)
                .MapError(error => new ParserError(ExceptionMessages.FloatExpected));

            VarParser = CreateRegexParser(ParserRegexes.GetVarParserRegex())
                .Map(VarParserMap)
                .MapError(error => new ParserError(ExceptionMessages.VariableExpected));

            OpenParanParser = CreateStringParser(ParserTokens.OpenBracket)
                .Map(OpenParanParserMap);

            CloseParanParser = CreateStringParser(ParserTokens.CloseBracket)
                .Map(CloseParanParserMap);

            OperatorParser = CreateChoiceParser(
                    CreateStringParser(ParserTokens.Plus), 
                    CreateStringParser(ParserTokens.Minus), 
                    CreateStringParser(ParserTokens.Multiply), 
                    CreateStringParser(ParserTokens.Divide),
                    CreateStringParser(ParserTokens.Amp))
                .Map(OperatorParserMap)
                .MapError(error => new ParserError(ExceptionMessages.OperatorExpected));
        }

        private static void CreateComplexParsers()
        {     
            CellRefParser = CreateSequenceOfParser(LettersParser, DigitsParser)
                .Map(CellRefParserMap)
                .MapError(error => new ParserError(ExceptionMessages.CellRefExpected));

            CellRangeParser = CreateSequenceOfParser(CellRefParser, CreateStringParser(ParserTokens.Colon), CellRefParser)
                .Map(CellRangeParserMap)
                .MapError(error => new ParserError(ExceptionMessages.CellRangeRefExpected));

            SheetNameCellRefParser = CreateSequenceOfParser(CreateManyMaxParser(CreateSequenceOfParser(VarParser, CreateStringParser(ParserTokens.Exclamation))), CellRefParser)
                .Map(SheetNameCellRefParserMap)
                .MapError(error => new ParserError(ExceptionMessages.CellRefExpected));

            SheetNameCellRangeParser = CreateSequenceOfParser(SheetNameCellRefParser, CreateStringParser(ParserTokens.Colon), SheetNameCellRefParser)
                .Map(SheetNameCellRangeParserMap)
                .MapError(error => new ParserError(ExceptionMessages.CellRangeRefExpected));

            FormulaArgumentParser = CreateLazyChoiceParser(new Lazy<Parser[]>(()=> new Parser[] { FormulaParser, FloatParser, NumberParser, StringValueParser, BoolValueParser, SheetNameCellRangeParser, SheetNameCellRefParser, VarParser }));

            FormulaParser = CreateSequenceOfParser(VarParser, CreateBetweenParser(OpenParanParser, CloseParanParser, CreateManySeptParser(FormulaArgumentParser, CreateRegexParser(ParserRegexes.GetFormulaParserRegex()))))
                .Map(FormulaParserMap)
                .MapError(error => new ParserError(ExceptionMessages.FormulaExpected));

            OperandParser = CreateChoiceParser(FormulaParser, SheetNameCellRangeParser, SheetNameCellRefParser, FloatParser, NumberParser, StringValueParser, BoolValueParser, VarParser);

            EquationParser = CreateManySeptParser(CreateChoiceParser(OperandParser, OperatorParser, OpenParanParser, CloseParanParser), SpaceParser);

            RangeFinderParser = CreateRangeFinderParser();
        }
        #endregion

        #region Maps
        private static ParserResult NumberParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.Number, (result as StringResult).Value);
        }

        private static ParserResult StringParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.String, (result as StringResult).Value);
        }

        private static ParserResult BoolParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.Bool, (result as StringResult).Value);
        }

        private static ParserResult FloatParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.Float, (result as StringResult).Value);
        }

        private static ParserResult VarParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.VarName, (result as StringResult).Value);
        }

        private static ParserResult OpenParanParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.OpenParan, ParserTokens.OpenBracket);
        }

        private static ParserResult CloseParanParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.CloseParan, ParserTokens.CloseBracket);
        }

        private static ParserResult OperatorParserMap(ParserResult result)
        {
            return new CalcParserResult(CalcParserResultKind.Operator, (result as StringResult).Value);
        }

        private static ParserResult CellRefParserMap(ParserResult result)
        {
            var res = result as ArrayResult;
            return new CalcParserResult(CalcParserResultKind.CellRef, (res.Value[0] as StringResult).Value + (res.Value[1] as StringResult).Value);
        }

        private static ParserResult CellRangeParserMap(ParserResult result)
        {
            var res = result as ArrayResult;
            return new CalcParserResult(CalcParserResultKind.CellRangeRef, (res.Value[0] as CalcParserResult).Value + ":" + (res.Value[2] as CalcParserResult).Value);
        }

        private static ParserResult SheetNameCellRefParserMap(ParserResult result)
        {
            var res = result as ArrayResult;
            string sheetName = "";
            var sheetNameSeqRes = ((res.Value[0] as ArrayResult).Value);
            if (sheetNameSeqRes.Length > 0)
            {
                sheetName = ((sheetNameSeqRes[0] as ArrayResult).Value[0] as CalcParserResult).Value;
            }
            string refStr = (res.Value[1] as CalcParserResult).Value;
            return new CalcParserResult(CalcParserResultKind.CellRef, sheetName + "!" + refStr);
        }

        private static ParserResult SheetNameCellRangeParserMap(ParserResult result)
        {
            var res = result as ArrayResult;
            return new CalcParserResult(CalcParserResultKind.CellRangeRef, (res.Value[0] as CalcParserResult).Value + ":" + (res.Value[2] as CalcParserResult).Value);
        }

        private static ParserResult FormulaParserMap(ParserResult result)
        {
            var res = result as ArrayResult;
            return new CalcParserResult(
                    CalcParserResultKind.Formula,
                    (res.Value[0] as CalcParserResult).Value.ToUpperInvariant(),
                    (res.Value[1] as ArrayResult).Value.Select<ParserResult, CalcParserResult>(el => {
                        return el as CalcParserResult;
                    }).ToArray()
                );
        }
        #endregion

        #region Parsers Factory
        private static Parser CreateRangeFinderParser()
        {
            return new ManyOccuranceParser(
                    CreateChoiceParser(
                        SheetNameCellRangeParser,
                        SheetNameCellRefParser
                    )
                );
        }

        private static Parser CreateStringParser(string value)
        {
            return new StringParser(value);
        }

        private static Parser CreateManyParser(Parser parser)
        {
            return new ManyParser(parser);
        }

        private static Parser CreateManyMaxParser(Parser parser)
        {
            return new ManyMaxParser(parser);
        }

        private static Parser CreateRegexParser(string pattern)
        {
            return new RegexParser(new Regex(pattern));
        }

        private static Parser CreateSequenceOfParser(params Parser[] parsers)
        {
            return new SequenceOfParser(parsers);
        }

        private static Parser CreateChoiceParser(params Parser[] parsers)
        {
            return new ChoiceParser(parsers);
        }

        private static Parser CreateLazyChoiceParser(Lazy<Parser[]> parsers)
        {
            return new ChoiceParser(parsers);
        }

        private static Parser CreateBetweenParser(Parser left, Parser right, Parser content)
        {
            return new BetweenParser(left, right, content);
        }

        private static Parser CreateManySeptParser(Parser parser, Parser septBy)
        {
            return new ManySeptParser(parser, septBy);
        }
        #endregion
    }
}
