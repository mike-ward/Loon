using Loon.Interfaces;
using Loon.ViewModels.Content.UserProfile;

namespace Loon.Commands
{
    public class SetUserProfileContextCommand : BaseCommand
    {
        private readonly ITwitterService twitterService;
        private readonly IPubSubService pubSubService;

        public SetUserProfileContextCommand(ITwitterService twitterService, IPubSubService pubSubService)
        {
            this.twitterService = twitterService;
            this.pubSubService = pubSubService;
        }

        public override void Execute(object? parameter)
        {
            pubSubService.Publish(UserProfileViewModel.SetUserProfileContextMessage, parameter);
        }
    }
}