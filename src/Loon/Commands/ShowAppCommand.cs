using Avalonia.Controls;

namespace Loon.Commands
{
    public sealed class ShowAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            App.MainWindow.WindowState = WindowState.Normal;
        }
    }
}