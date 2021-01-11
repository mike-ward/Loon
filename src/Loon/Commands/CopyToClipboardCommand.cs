using Avalonia;
using Loon.Extensions;

namespace Loon.Commands
{
    public class CopyToClipboardCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter is string text)
            {
                Application.Current.Clipboard.SetTextAsync(text).FireAndForget();
            }
        }
    }
}