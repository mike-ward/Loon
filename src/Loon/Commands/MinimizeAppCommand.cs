using Avalonia.Controls;

namespace Loon.Commands
{
    public class MinimizeAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            App.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}