using System;
using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Loon.Services;
using Twitter.Models;

namespace Loon.Commands
{
    internal class LikesAddRemoveCommand : BaseCommand
    {
        private bool            inCommand;
        private ITwitterService TwitterService { get; }

        public LikesAddRemoveCommand(ITwitterService twitterService)
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

                    if (status.Favorited)
                    {
                        await TwitterService.TwitterApi.DestroyFavorite(status.Id).ConfigureAwait(true);
                        status.Favorited     = false;
                        status.FavoriteCount = Math.Max(0, status.FavoriteCount - 1);
                    }
                    else
                    {
                        await TwitterService.TwitterApi.CreateFavorite(status.Id).ConfigureAwait(true);
                        status.Favorited = true;
                        status.FavoriteCount++;
                    }

                    PubSubs.UpdateLikesTimeline.Publish(new Unit());
                }
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}