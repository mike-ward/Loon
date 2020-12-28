using Avalonia.Controls;
using Avalonia.VisualTree;
using Loon.Behaviors;

namespace Loon.Commands
{
    public class TabGoBackCommand : BaseCommand
    {
        public static readonly TabGoBackCommand Command = new TabGoBackCommand();

        public override void Execute(object? parameter)
        {
            var tabControl = App.MainWindow.FindDescendantOfType<TabControl>();
            tabControl.SelectedIndex = PreviousIndexBehavior.GetPreviousIndex(tabControl);
        }
    }
}