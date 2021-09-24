using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    internal class SettingsOpenSettings : UserControl
    {
        public SettingsOpenSettings()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenSettings(string filename)
        {
            Services.OpenUrlService.Open(filename);
        }
    }
}