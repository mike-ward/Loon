using System;
using Loon.Interfaces;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    internal class ClearHiddenImageCacheCommand : BaseCommand
    {
        private ISettings Settings { get; }

        public ClearHiddenImageCacheCommand(ISettings settings)
        {
            Settings = settings;
        }

        public override async void Execute(object? parameter)
        {
            if (await MessageBox.Show(App.GetString("settings-sure"), MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
            {
                Settings.HiddenImagesSet.Clear();
                Settings.Save();
                Console.Beep();
            }
        }
    }
}