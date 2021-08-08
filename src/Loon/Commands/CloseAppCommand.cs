namespace Loon.Commands
{
    internal class CloseAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            App.MainWindow.Close();
        }
    }
}