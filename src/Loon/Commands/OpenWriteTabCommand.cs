using Loon.Services;
using Twitter.Models;

namespace Loon.Commands
{
    public class OpenWriteTabCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            PubSubs.OpenWriteTab.Publish(parameter as TwitterStatus);
        }
    }
}