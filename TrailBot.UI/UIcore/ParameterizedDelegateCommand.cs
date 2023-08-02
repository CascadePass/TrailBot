using System;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI
{
    public class ParameterizedDelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public ParameterizedDelegateCommand(Action<object> action)
        {
            this.Action = action;
        }

        public Action<object> Action { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.Action.Invoke(parameter);
        }
    }
}
