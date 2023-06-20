using System;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action action)
        {
            this.Action = action;
        }

        public Action Action { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.Action.Invoke();
        }
    }
}
