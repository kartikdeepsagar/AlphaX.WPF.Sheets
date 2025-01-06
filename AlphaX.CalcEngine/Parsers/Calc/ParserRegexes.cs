namespace AlphaX.CalcEngine.Parsers
{
    internal static class ParserRegexes
    {
        public static string GetFloatParserRegex()
        {
            return @"^\d*\.\d+";
        }

        public static string GetFormulaParserRegex()
        {
            return @"^\s*,\s*";
        }

        public static string GetVarParserRegex()
        {
            return @"^[a-zA-Z]+[\w\d]*";
        }
    }
}
