using Loon.Interfaces;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    internal class AddToHiddenImagesCommand : BaseCommand
    {
        private ISettings Settings { get; }

        public AddToHiddenImagesCommand(ISettings settings)
        {
            Settings = settings;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter is string url &&
                await MessageBox.Show(App.GetString("always-hide-image"), MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes &&
                Settings.HiddenImagesSet.Add(url))
            {
                Settings.Save();
            }
        }
    }
}