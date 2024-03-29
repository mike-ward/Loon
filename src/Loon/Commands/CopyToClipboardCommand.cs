﻿using Avalonia;
using Loon.Extensions;

namespace Loon.Commands
{
    public sealed class CopyToClipboardCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter is string text &&
                Application.Current?.Clipboard is not null)
            {
                var unused = Application.Current.Clipboard.SetTextAsync(text).FireAndForget();
            }
        }
    }
}