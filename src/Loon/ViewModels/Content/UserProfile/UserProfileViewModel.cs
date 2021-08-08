using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Loon.Services;
using Twitter.Models;

namespace Loon.ViewModels.Content.UserProfile
{
    internal class UserProfileViewModel : NotifyPropertyChanged
    {
        private readonly ITwitterService twitterService;

        private User? user;

        public User? UserProfileContext
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public UserProfileViewModel(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
            PubSubs.SetUserProfileContext.Subscribe(UserProfileContextHandler);
        }

        private void UserProfileContextHandler(object? payload)
        {
            if (payload is User user)
            {
                UserProfileContext = user;
            }
            else if (payload is string screenName)
            {
                GetUserInfo(screenName).FireAndForget();
            }
            else
            {
                UserProfileContext = null;
            }
        }

        private async ValueTask GetUserInfo(string screenName)
        {
            UserProfileContext = await twitterService.UserInfo(screenName).ConfigureAwait(true);
        }
    }
}