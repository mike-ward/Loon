using Avalonia;
using Loon.Interfaces;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    public class AddToHiddenImagesCommand : BaseCommand
    {
        private ISettings Settings { get; }

        public AddToHiddenImagesCommand(ISettings settings)
        {
            Settings = settings;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter is (string url, PixelPoint pixelPoint) &&
                await MessageBox.Show(App.GetString("always-hide-image"), MessageBox.MessageBoxButtons.YesNo, pixelPoint) == MessageBox.MessageBoxResult.Yes &&
                Settings.HiddenImagesSet.Add(url))
            {
                Settings.Save();
            }
        }
    }
}