using System;
using System.Windows.Input;
using Avalonia.LogicalTree;
using Loon.ViewModels.Content.Write;
using Loon.Views.Content.Write;
using Twitter.Models;

namespace Loon.Commands
{
    internal class ReplyToCommand : ICommand
    {
        public static readonly ReplyToCommand Command = new();

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var content = App.MainWindow.Content as ILogical;

            if (content.FindLogicalDescendantOfType<WriteView>() is WriteView writeView &&
                writeView.DataContext is WriteViewModel writeViewModel)
            {
                GoToWriteTabCommand.Command.Execute(null);
                writeViewModel.ReplyTo = parameter as TwitterStatus;
            }
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public event EventHandler? CanExecuteChanged;
    }
}