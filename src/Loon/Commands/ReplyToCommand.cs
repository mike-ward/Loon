using Avalonia.LogicalTree;
using Loon.ViewModels.Content.Write;
using Loon.Views.Content.Write;
using Twitter.Models;

namespace Loon.Commands
{
    internal class ReplyToCommand : BaseCommand
    {
        public static readonly ReplyToCommand Command = new();

        public override void Execute(object? parameter)
        {
            var content = App.MainWindow.Content as ILogical;

            if (content.FindLogicalDescendantOfType<WriteView>() is WriteView writeView &&
                writeView.DataContext is WriteViewModel writeViewModel)
            {
                GoToWriteTabCommand.Command.Execute(null);
                writeViewModel.ReplyTo = parameter as TwitterStatus;
            }
        }
    }
}