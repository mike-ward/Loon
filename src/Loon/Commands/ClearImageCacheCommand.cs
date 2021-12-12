using System;
using Loon.Services;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    public class ClearImageCacheCommand : BaseCommand
    {
        public override async void Execute(object? parameter)
        {
            if (await MessageBox.Show(App.GetString("settings-sure"), MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
            {
                ImageService.ClearImageCache();
                Console.Beep();
            }
        }
    }
}