namespace AlphaX.CalcEngine.Parsers
{
    internal class ParserError
    {
        public string Message { get; set; }

        public ParserError(string message)
        {
            Message = message;
        }
    }
}
