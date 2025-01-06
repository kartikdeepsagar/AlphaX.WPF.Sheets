namespace AlphaX.CalcEngine
{
    public class CalcError
    {
        public string Message { get; private set; }
        public string Error { get; private set; }

        public CalcError(string msg, string err)
        {
            Message = msg;
            Error = err;
        }

    }

    public class ValueError: CalcError
    {
        public ValueError(): base("A value used in the formula is not of correct data type", "#Value!")
        {
            
        }
    }

    public class NameError : CalcError
    {
        public NameError() : base("The formula contains unrecognized text", "#Name!")
        {

        }
    }

    public class DivideByZeroError : CalcError
    {
        public DivideByZeroError() : base("Trying to divide value by 0", "#Div/0!")
        {

        }
    }
}
