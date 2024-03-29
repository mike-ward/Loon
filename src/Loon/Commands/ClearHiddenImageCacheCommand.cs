using System;
using System.Diagnostics.CodeAnalysis;
using Loon.Interfaces;
using Loon.Services;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    public sealed class ClearHiddenImageCacheCommand : BaseCommand
    {
        private ISettings Settings { get; }

        public ClearHiddenImageCacheCommand(ISettings settings)
        {
            Settings = settings;
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        public override async void Execute(object? parameter)
        {
            try
            {
                if (await MessageBox.Show(App.GetString("settings-sure"), MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
                {
                    Settings.HiddenImagesSet.Clear();
                    Settings.Save();
                    Console.Beep();
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}