namespace AlphaX.Sheets.Filtering
{
    public enum NumericFilterCriteria
    {
        Equals,
        GreaterThan,
        LessThan,
        LessThanOrEquals,
        GreaterThanOrEquals,
    }

    public class NumericFilter : FilterBase
    {
        public object[] Values { get; set; }
        public NumericFilterCriteria Criteria { get; set; }

        protected override bool Filter(object value)
        {
            return true;
        }
    }
}
