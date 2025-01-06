namespace AlphaX.CalcEngine.Parsers
{
    public enum CalcParserResultKind
    {
        Number,
        Float,
        VarName,
        CellRef,
        CellRangeRef,
        Operator,
        OpenParan,
        CloseParan,
        Formula,
        String,
        Bool
    }
}
