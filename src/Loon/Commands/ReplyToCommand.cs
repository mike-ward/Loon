using Avalonia.LogicalTree;
using Loon.Interfaces;
using Loon.ViewModels.Content.Write;
using Loon.Views.Content.Write;
using Twitter.Models;

namespace Loon.Commands
{
    public class ReplyToCommand : BaseCommand
    {
        private readonly ITwitterService _twitterService;

        public ReplyToCommand(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        public override void Execute(object? parameter)
        {
            var content = App.MainWindow.Content as ILogical;

            if (content.FindLogicalDescendantOfType<WriteView>() is WriteView writeView &&
                writeView.DataContext is WriteViewModel writeViewModel)
            {
                App.Commands.GoToWriteTab.Execute(null);
                writeViewModel.ReplyTo = parameter as TwitterStatus;
            }
        }
    }
}