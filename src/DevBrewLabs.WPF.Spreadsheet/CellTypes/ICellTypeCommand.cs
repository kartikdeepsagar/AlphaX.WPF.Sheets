namespace DevBrewLabs.WPF.Spreadsheet.CellTypes
{
    public interface ICellTypeCommand
    {
        bool CanExecute();
        void Execute(int row, int column);
    }
}