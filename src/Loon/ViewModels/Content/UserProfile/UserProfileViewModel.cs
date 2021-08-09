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

        private User? userProfileContext;

        public User? UserProfileContext
        {
            get => userProfileContext;
            set => SetProperty(ref userProfileContext, value);
        }

        public UserProfileViewModel(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
            PubSubs.SetUserProfileContext.Subscribe(UserProfileContextHandler);
        }

        private void UserProfileContextHandler(object? payload)
        {
            switch (payload)
            {
                case User user:
                    UserProfileContext = user;
                    break;
                case string screenName:
                    GetUserInfo(screenName).FireAndForget();
                    break;
                default:
                    UserProfileContext = null;
                    break;
            }
        }

        private async ValueTask GetUserInfo(string screenName)
        {
            UserProfileContext = await twitterService.UserInfo(screenName).ConfigureAwait(true);
        }
    }
}