namespace AlphaX.Sheets
{
    public abstract class FilterBase
    {
        internal bool PassesFilter(object value)
        {
            return Filter(value);
        }

        /// <summary>
        /// Evaluates whether the value passes the filter or not.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="cellRange"></param>
        /// <returns></returns>
        protected abstract bool Filter(object value);
    }
}
