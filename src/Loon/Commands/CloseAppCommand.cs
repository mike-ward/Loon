using Loon.Services;

namespace Loon.Commands
{
    public class CloseAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            ImageService.KillImageViewer();
            App.MainWindow.Close();
        }
    }
}