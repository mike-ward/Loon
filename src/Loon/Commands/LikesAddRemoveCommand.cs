using System.Threading.Tasks;
using Loon.Extensions;
using Twitter.Models;

namespace Loon.Commands
{
    internal class LikesAddRemoveCommand : BaseCommand
    {
        public static readonly LikesAddRemoveCommand Command = new();

        public override void Execute(object? parameter)
        {
            if (parameter is TwitterStatus status)
            {
                ExecuteAsync(status).FireAndForget();
            }
        }

        private async ValueTask ExecuteAsync(TwitterStatus status)
        {
            if (status.Favorited)
            {
                await TwitterService.DestroyFavorite(status.Id).ConfigureAwait(true);
                status.Favorited = false;
            }
            else
            {
                await TwitterService.CreateFavorite(status.Id).ConfigureAwait(true);
                status.Favorited = true;
            }
        }
    }
}