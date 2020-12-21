using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsOpenSettings : UserControl
    {
        public SettingsOpenSettings()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenSettings(string filename)
        {
            Services.OpenUrlService.Open(filename);
        }
    }
}