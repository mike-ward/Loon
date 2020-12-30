using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Loon.Commands
{
    public class GoToWriteTabCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            var tabControl = App.MainWindow.FindDescendantOfType<TabControl>();
            tabControl.SelectedIndex = tabControl.ItemCount - 1;
        }
    }
}