namespace DevBrewLabs.WPF.Spreadsheet.Commands
{
    public class CommitEditCommand : SpreadCommand
    {
        public CommitEditCommand(Spread spread) : base(spread)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Spread.EditingManager.IsEditing;
        }

        public override void Execute(object parameter)
        {
            Spread.EditingManager.EndEdit(true);
        }
    }
}
