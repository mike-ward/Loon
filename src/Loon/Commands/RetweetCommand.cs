using System;
using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.Commands
{
    public class RetweetCommand : BaseCommand
    {
        private bool            inCommand;
        private ISettings       Settings       { get; }
        private ITwitterService TwitterService { get; }

        public RetweetCommand(ISettings settings, ITwitterService twitterService)
        {
            Settings       = settings;
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

                    if (status is not null &&
                        !status.IsMyTweet &&
                        status.OriginatingStatus.User.ScreenName.IsNotEqualTo(Settings.ScreenName))
                    {
                        if (status.IsMyTweet) return;

                        if (status.RetweetedByMe)
                        {
                            await TwitterService.UnretweetStatus(status.Id).ConfigureAwait(true);
                            status.RetweetCount  = Math.Max(0, status.RetweetCount - 1);
                            status.RetweetedByMe = false;
                        }
                        else
                        {
                            await TwitterService.RetweetStatus(status.Id).ConfigureAwait(true);
                            status.RetweetCount++;
                            status.RetweetedByMe = true;
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