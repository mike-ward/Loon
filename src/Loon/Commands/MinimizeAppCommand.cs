using Avalonia.Controls;

namespace Loon.Commands
{
    internal class MinimizeAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            App.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}