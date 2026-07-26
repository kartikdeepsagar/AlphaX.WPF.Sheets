using System;
using System.Windows.Input;

namespace DevBrewLabs.WPF.Spreadsheet.Commands
{
    public class SpreadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Spread Spread { get; }

        public SpreadCommand(Spread spread)
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
