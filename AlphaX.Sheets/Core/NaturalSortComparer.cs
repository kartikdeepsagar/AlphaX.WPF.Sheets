using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AlphaX.Sheets
{
    internal class NaturalSortComparer : IComparer<KeyValuePair<int, Cell>>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        static extern int StrCmpLogicalW(string s1, string s2);

        public int Compare(KeyValuePair<int, Cell> x, KeyValuePair<int, Cell> y)
        {
            return StrCmpLogicalW(x.Value == null || x.Value.Value == null ? string.Empty : x.Value.Value.ToString(),
                y.Value == null || y.Value.Value == null ? string.Empty : y.Value.Value.ToString());
        }
    }
}
