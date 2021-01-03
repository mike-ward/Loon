using Loon.Interfaces;
using Loon.ViewModels.Content.Write;

namespace Loon.Commands
{
    public class OpenWriteTabCommand : BaseCommand
    {
        private readonly IPubSubService pubSubService;

        public OpenWriteTabCommand(IPubSubService pubSubService)
        {
            this.pubSubService = pubSubService;
        }

        public override void Execute(object? parameter)
        {
            pubSubService.Publish(WriteViewModel.OpenWriteTabMessage, parameter);
        }
    }
}