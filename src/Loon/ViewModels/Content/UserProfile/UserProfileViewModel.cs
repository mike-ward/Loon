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

        public User? UserProfileContext { get => Getter(default(User)); set => Setter(value); }

        public UserProfileViewModel(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
            PubSubService.AddSubscriber(PubSubService.SetUserProfileContextMessage, UserProfileContextHandler);
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