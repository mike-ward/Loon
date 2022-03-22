using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Loon.Interfaces;
using Loon.Services;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    public sealed class AddToHiddenImagesCommand : BaseCommand
    {
        private ISettings Settings { get; }

        public AddToHiddenImagesCommand(ISettings settings)
        {
            Settings = settings;
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        public override async void Execute(object? parameter)
        {
            try
            {
                if (parameter is (string url, PixelPoint pixelPoint) &&
                    await MessageBox.Show(App.GetString("always-hide-image"), MessageBox.MessageBoxButtons.YesNo, pixelPoint) == MessageBox.MessageBoxResult.Yes &&
                    Settings.HiddenImagesSet.Add(url))
                {
                    Settings.Save();
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}