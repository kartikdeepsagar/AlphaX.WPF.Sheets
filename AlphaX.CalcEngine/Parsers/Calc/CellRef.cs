using System;
using System.Text;

namespace AlphaX.CalcEngine.Parsers
{
    internal class CellRef : IEquatable<CellRef>
    {
        private readonly string _rangeName;

        public string Name => string.IsNullOrEmpty(SheetName) ? _rangeName : SheetName + "!" + _rangeName;

        public int Column { get; }
        public int Row { get; }
        public string SheetName { get; }

        public CellRef(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string cellRef = name;
            string sheetName = string.Empty;

            int exclamationIdx = name.LastIndexOf('!');
            if (exclamationIdx >= 0)
            {
                sheetName = name.Substring(0, exclamationIdx);
                cellRef = name.Substring(exclamationIdx + 1);
            }

            int digitIdx = -1;
            for (int i = 0; i < cellRef.Length; i++)
            {
                if (char.IsDigit(cellRef[i]))
                {
                    digitIdx = i;
                    break;
                }
            }

            if (digitIdx <= 0 || !int.TryParse(cellRef.Substring(digitIdx), out int rowNumber))
            {
                throw new ArgumentException($"Invalid cell reference format: '{name}'", nameof(name));
            }

            string colLetter = cellRef.Substring(0, digitIdx);
            _rangeName = colLetter.ToUpperInvariant() + rowNumber.ToString();
            Column = GetColumnNumberFromLetter(colLetter) - 1;
            Row = rowNumber - 1;
            SheetName = sheetName;
        }

        public CellRef(int row, int col, string sheetName = "")
        {
            Row = row;
            Column = col;
            _rangeName = GetColumnLetterFromNumber(col + 1) + (row + 1).ToString();
            SheetName = sheetName ?? string.Empty;
        }

        public static int GetColumnNumberFromLetter(string letter)
        {
            if (string.IsNullOrEmpty(letter))
                return 0;

            int col = 0;
            foreach (char c in letter)
            {
                if (c >= 'A' && c <= 'Z')
                    col = col * 26 + (c - 'A' + 1);
                else if (c >= 'a' && c <= 'z')
                    col = col * 26 + (c - 'a' + 1);
            }
            return col;
        }

        public static string GetColumnLetterFromNumber(int num)
        {
            if (num <= 0)
                return string.Empty;

            var result = new StringBuilder();
            while (num > 0)
            {
                int remainder = (num - 1) % 26;
                result.Insert(0, (char)('A' + remainder));
                num = (num - 1) / 26;
            }
            return result.ToString();
        }

        #region Equals Comparison

        public override bool Equals(object obj) => Equals(obj as CellRef);

        public bool Equals(CellRef other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Row == other.Row &&
                   Column == other.Column &&
                   string.Equals(SheetName, other.SheetName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Row;
                hash = hash * 31 + Column;
                hash = hash * 31 + (SheetName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(SheetName) : 0);
                return hash;
            }
        }

        public static bool operator ==(CellRef lhs, CellRef rhs)
        {
            if (lhs is null) return rhs is null;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CellRef lhs, CellRef rhs) => !(lhs == rhs);

        #endregion
    }
}
