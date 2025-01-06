using System;
using System.Windows.Input;

namespace AlphaX.WPF.Sheets.Commands
{
    public class AlphaXSpreadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AlphaXSpread Spread { get; }

        public AlphaXSpreadCommand(AlphaXSpread spread)
        {
            Spread = spread;
        }

        public virtual bool CanExecute(object parameter)
        {
            return false;
        }

        public virtual void Execute(object parameter)
        {
            
        }
    }
}
