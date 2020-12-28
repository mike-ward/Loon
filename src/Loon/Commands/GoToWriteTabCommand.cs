using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Loon.Commands
{
    internal class GoToWriteTabCommand : BaseCommand
    {
        public static readonly GoToWriteTabCommand Command = new();

        public override void Execute(object? parameter)
        {
            var tabControl = App.MainWindow.FindDescendantOfType<TabControl>();
            tabControl.SelectedIndex = tabControl.ItemCount - 1;
        }
    }
}