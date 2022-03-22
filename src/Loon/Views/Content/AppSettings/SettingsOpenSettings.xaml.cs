using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Services;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsOpenSettings : UserControl
    {
        public SettingsOpenSettings()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenSettings(string filename)
        {
            OpenUrlService.Open(filename);
        }
    }
}