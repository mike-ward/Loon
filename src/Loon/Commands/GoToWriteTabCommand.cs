using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Loon.Commands
{
    internal class GoToWriteTabCommand : ICommand
    {
        public static readonly GoToWriteTabCommand Command = new();

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var tabControl = App.MainWindow.FindDescendantOfType<TabControl>();
            tabControl.SelectedIndex = tabControl.ItemCount - 1;
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public event EventHandler? CanExecuteChanged;
    }
}