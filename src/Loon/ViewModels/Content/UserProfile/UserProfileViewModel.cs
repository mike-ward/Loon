using System.Threading.Tasks;
using Loon.Interfaces;
using Loon.Models;
using Loon.Services;
using Twitter.Models;

namespace Loon.ViewModels.Content.UserProfile
{
    public class UserProfileViewModel : NotifyPropertyChanged
    {
        private readonly ITwitterService twitterService;

        private User? user;
        public User? UserProfileContext { get => user; set => SetProperty(ref user, value); }

        public UserProfileViewModel(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
            PubSubs.SetUserProfileContext.Subscribe(UserProfileContextHandler);
        }

        private void UserProfileContextHandler(object? payload)
        {
            UserProfileContext = payload switch
            {
                User user => user,
                string screenName => Task.Run(async () => await twitterService.UserInfo(screenName).ConfigureAwait(false)).Result,
                _ => null
            };
        }
    }
}