using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Loon.Interfaces;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    public class ClearHiddenImageCacheCommand : BaseCommand
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
                Trace.WriteLine(ex);
            }
        }
    }
}