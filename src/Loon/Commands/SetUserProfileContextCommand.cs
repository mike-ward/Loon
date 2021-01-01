using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.ViewModels;
using Twitter.Models;

namespace Loon.Commands
{
    public class SetUserProfileContextCommand : BaseCommand
    {
        private readonly ITwitterService twitterService;

        public SetUserProfileContextCommand(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
        }

        public override void Execute(object? parameter)
        {
            ExecuteAsync(parameter).FireAndForget();
        }

        public async ValueTask ExecuteAsync(object? parameter)
        {
            if (App.MainWindow.DataContext is MainWindowViewModel vm)
            {
                vm.UserProfileContext = parameter switch
                {
                    User user => user,
                    string screenName => await twitterService.UserInfo(screenName).ConfigureAwait(true),
                    _ => null
                };
            }
        }
    }
}