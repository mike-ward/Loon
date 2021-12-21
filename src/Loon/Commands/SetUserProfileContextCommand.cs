using Loon.Services;

namespace Loon.Commands
{
    public class SetUserProfileContextCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            PubSubs.SetUserProfileContext.Publish(parameter);
        }
    }
}