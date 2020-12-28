using System;
using System.Windows.Input;
using Loon.Interfaces;
using Loon.Services;

namespace Loon.Commands
{
    public class BaseCommand : ICommand
    {
        protected static ITwitterService TwitterService { get; } = BootStrapper.GetService<ITwitterService>();

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public event EventHandler? CanExecuteChanged;
    }
}