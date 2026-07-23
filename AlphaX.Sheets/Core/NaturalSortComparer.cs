using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AlphaX.Sheets
{
    /// <summary>
    /// Natural sort comparer adhering to Excel sort rules:
    /// 1. Blank/null cells always sort to the bottom in both Ascending &amp; Descending.
    /// 2. Data type hierarchy: Numbers &lt; Text &lt; Booleans.
    /// 3. Natural alphanumeric string comparison (StrCmpLogicalW).
    /// </summary>
    internal class NaturalSortComparer : IComparer<object>, IComparer<Cells.RowSnapshot>, IComparer
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);

        private readonly bool _ascending;

        public NaturalSortComparer() : this(true)
        {
        }

        public NaturalSortComparer(bool ascending)
        {
            _ascending = ascending;
        }

        public int Compare(Cells.RowSnapshot snapX, Cells.RowSnapshot snapY)
        {
            return Compare(snapX.KeyValue, snapY.KeyValue);
        }

        public int Compare(object x, object y)
        {
            if (x is Cells.RowSnapshot snapX && y is Cells.RowSnapshot snapY)
            {
                x = snapX.KeyValue;
                y = snapY.KeyValue;
            }

            bool isNullX = IsBlank(x);
            bool isNullY = IsBlank(y);

            if (isNullX && isNullY)
                return 0;

            // Excel rule: Blank cells ALWAYS sort to the bottom in both Ascending & Descending
            if (isNullX)
                return 1;
            if (isNullY)
                return -1;

            int typeOrderX = GetDataTypeOrder(x);
            int typeOrderY = GetDataTypeOrder(y);

            int result;
            if (typeOrderX != typeOrderY)
            {
                result = typeOrderX.CompareTo(typeOrderY);
            }
            else
            {
                result = CompareSameType(x, y);
            }

            return _ascending ? result : -result;
        }

        private static bool IsBlank(object val)
        {
            if (val == null || val == DBNull.Value)
                return true;

            if (val is string str && string.IsNullOrWhiteSpace(str))
                return true;

            return false;
        }

        private static int GetDataTypeOrder(object val)
        {
            if (IsNumeric(val))
                return 1;
            if (val is bool)
                return 3;

            return 2; // Text / other objects
        }

        private static bool IsNumeric(object val)
        {
            return val is sbyte || val is byte || val is short || val is ushort ||
                   val is int || val is uint || val is long || val is ulong ||
                   val is float || val is double || val is decimal;
        }

        private static int CompareSameType(object x, object y)
        {
            if (IsNumeric(x) && IsNumeric(y))
            {
                double d1 = Convert.ToDouble(x);
                double d2 = Convert.ToDouble(y);
                return d1.CompareTo(d2);
            }

            if (x is bool b1 && y is bool b2)
            {
                return b1.CompareTo(b2);
            }

            string s1 = x.ToString();
            string s2 = y.ToString();

            try
            {
                return StrCmpLogicalW(s1, s2);
            }
            catch
            {
                return string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
