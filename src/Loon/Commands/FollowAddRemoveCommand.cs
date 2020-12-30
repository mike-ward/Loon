using System;
using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.Commands
{
    public class FollowAddRemoveCommand : BaseCommand
    {
        private bool inCommand;
        private ITwitterService TwitterService { get; }

        public FollowAddRemoveCommand(ITwitterService twitterService)
        {
            TwitterService = twitterService;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is TwitterStatus status)
            {
                ExecuteAsync(status).FireAndForget();
            }
        }

        private async ValueTask ExecuteAsync(TwitterStatus status)
        {
            try
            {
                if (!inCommand)
                {
                    inCommand = true;
                    var user = status.User;
                    var screenName = user.ScreenName;

                    if (screenName is not null)
                    {
                        if (status.User.IsFollowing)
                        {
                            await TwitterService.Unfollow(screenName).ConfigureAwait(true);
                            user.Followers = Math.Max(0, user.Followers - 1);
                            user.IsFollowing = false;
                        }
                        else
                        {
                            await TwitterService.Follow(screenName).ConfigureAwait(true);
                            user.Followers++;
                            user.IsFollowing = true;
                        }
                    }
                }
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}