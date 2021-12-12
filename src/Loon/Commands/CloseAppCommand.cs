namespace Loon.Commands
{
    public class CloseAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            App.MainWindow.Close();
        }
    }
}