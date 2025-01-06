using System;

namespace AlphaX.Sheets.Filtering
{
    public enum StringFilterCriteria
    {
        Equals,
        Contains,
        StartsWith,
        EndsWidth
    }

    public sealed class StringFilter : FilterBase
    {
        public bool MatchCase { get; set; }
        public string[] Values { get; set; }
        public StringFilterCriteria Criteria { get; set; }

        protected override bool Filter(object value)
        {
            if (Values == null || Values.Length == 0)
                return true;

            foreach(var item in Values)
            {
                if (item == null && value == null)
                    return true;

                if (item == null && value != null)
                    return false;

                if (item != null && value == null)
                    return false;

                switch (Criteria)
                {
                    case StringFilterCriteria.Equals:
                        return string.Equals(value?.ToString(), item, 
                            MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

                    case StringFilterCriteria.Contains:
                        return value.ToString().IndexOf(item, 
                            MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0;

                    case StringFilterCriteria.StartsWith:
                        return value.ToString().StartsWith(item,
                            MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

                    case StringFilterCriteria.EndsWidth:
                        return value.ToString().EndsWith(item,
                            MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                }
            }

            return true;
        }
    }
}
