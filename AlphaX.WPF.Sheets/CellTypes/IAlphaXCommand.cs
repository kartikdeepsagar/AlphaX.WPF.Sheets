namespace AlphaX.WPF.Sheets.CellTypes
{
    public interface IAlphaXCommand
    {
        bool CanExecute();
        void Execute(int row, int column);
    }
}