namespace AlphaX.WPF.Sheets.Commands
{
    public class CancelEditCommand : AlphaXSpreadCommand
    {
        public CancelEditCommand(AlphaXSpread spread) : base(spread)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Spread.EditingManager.IsEditing;
        }

        public override void Execute(object parameter)
        {
            Spread.EditingManager.EndEdit(false);
        }
    }
}
