using System;
using System.Windows.Input;

namespace sUpdater.Models
{
    public class LinkClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action execute;

        public LinkClickCommand(Action execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke();
        }
    }
}
