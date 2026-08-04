using DevBrewLabs.Spreadsheet;

namespace DevBrewLabs.WPF.Spreadsheet
{
    internal class State
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public CellRange Selection { get; set; }
        public object Value { get; set; }
    }
}
