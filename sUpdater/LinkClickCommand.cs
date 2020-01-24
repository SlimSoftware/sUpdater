using System;
using System.Windows.Input;

namespace sUpdater
{
    public class LinkClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action excute;

        public LinkClickCommand(Action excute)
        {
            this.excute = excute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            excute.Invoke();
        }
    }
}
