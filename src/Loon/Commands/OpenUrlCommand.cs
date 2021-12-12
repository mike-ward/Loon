using Loon.Services;

namespace Loon.Commands
{
    public class OpenUrlCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter is string link)
            {
                OpenUrlService.Open(link);
            }
        }
    }
}