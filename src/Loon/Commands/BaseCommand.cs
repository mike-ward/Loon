using System;
using System.Windows.Input;

namespace Loon.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract void       Execute(object?    parameter);
        public          bool       CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged;
        protected void             OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}