using Loon.Services;

namespace Loon.Commands
{
    public class OpenWriteTabCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            PubSubService.Publish(PubSubService.OpenWriteTabMessage, parameter);
        }
    }
}