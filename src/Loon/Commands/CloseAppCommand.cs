﻿using Loon.Services;

namespace Loon.Commands
{
    public sealed class CloseAppCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            ImageService.KillImageViewer();
            App.MainWindow.Close();
        }
    }
}