using Loon.Interfaces;

namespace Loon.Commands
{
    public class SignoutCommand : BaseCommand
    {
        private readonly ISettings _settings;

        public SignoutCommand(ISettings settings)
        {
            _settings = settings;
        }

        public override void Execute(object? parameter)
        {
            _settings.AccessToken = null;
            _settings.AccessTokenSecret = null;
            _settings.ScreenName = null;
        }
    }
}