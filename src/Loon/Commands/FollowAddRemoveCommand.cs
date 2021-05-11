using System;
using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.Commands
{
    public class FollowAddRemoveCommand : BaseCommand
    {
        private bool            inCommand;
        private ITwitterService TwitterService { get; }

        public FollowAddRemoveCommand(ITwitterService twitterService)
        {
            TwitterService = twitterService;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is User user)
            {
                ExecuteAsync(user).FireAndForget();
            }
        }

        private async ValueTask ExecuteAsync(User user)
        {
            try
            {
                if (!inCommand)
                {
                    inCommand = true;
                    var screenName = user.ScreenName;

                    if (screenName is not null)
                    {
                        if (user.IsFollowing)
                        {
                            await TwitterService.Unfollow(screenName).ConfigureAwait(true);
                            user.Followers   = Math.Max(0, user.Followers - 1);
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