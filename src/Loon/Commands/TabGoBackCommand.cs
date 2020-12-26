using System;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;

namespace Loon.Commands
{
    public class TabGoBackCommand : ICommand
    {
        public static readonly TabGoBackCommand Command = new TabGoBackCommand();

        public TabItem? LastSelectedTab { get; set; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var tc = LastSelectedTab?.Parent as TabControl;

            if (tc is not null)
            {
                tc.SelectedItem = LastSelectedTab ?? tc.Items.Cast<TabItem>().First();
            }
        }

        public event EventHandler? CanExecuteChanged;

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }
    }
}