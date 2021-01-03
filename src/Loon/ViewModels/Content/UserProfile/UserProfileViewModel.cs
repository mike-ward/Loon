using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.UserProfile
{
    public class UserProfileViewModel : NotifyPropertyChanged
    {
        public const string SetUserProfileContextMessage = "set-user-profile-message";
        private readonly ITwitterService twitterService;

        public User? UserProfileContext { get => Getter(default(User)); set => Setter(value); }

        public UserProfileViewModel(IPubSubService pubSubService, ITwitterService twitterService)
        {
            pubSubService.PubSubRaised += UserProfileContextHandler;
            this.twitterService = twitterService;
        }

        private async void UserProfileContextHandler(object? sender, PubSubEventArgs e)
        {
            if (e.Message.IsEqualTo(SetUserProfileContextMessage))
            {
                UserProfileContext = e.Payload switch
                {
                    User user => user,
                    string screenName => await twitterService.UserInfo(screenName).ConfigureAwait(false),
                    _ => null
                };
            }
        }
    }
}