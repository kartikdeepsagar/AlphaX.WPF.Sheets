namespace AlphaX.WPF.Sheets.Commands
{
    public class CommitEditCommand : AlphaXSpreadCommand
    {
        public CommitEditCommand(AlphaXSpread spread) : base(spread)
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
