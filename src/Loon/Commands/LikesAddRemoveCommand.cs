using System;
using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.Commands
{
    public class LikesAddRemoveCommand : BaseCommand
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
                        await TwitterService.DestroyFavorite(status.Id).ConfigureAwait(true);
                        status.Favorited     = false;
                        status.FavoriteCount = Math.Max(0, status.FavoriteCount - 1);
                    }
                    else
                    {
                        await TwitterService.CreateFavorite(status.Id).ConfigureAwait(true);
                        status.Favorited = true;
                        status.FavoriteCount++;
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