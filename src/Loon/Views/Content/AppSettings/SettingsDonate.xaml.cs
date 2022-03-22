using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Services;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsDonate : UserControl
    {
        public SettingsDonate()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnDonate(string par)
        {
            OpenUrlService.Open(par);
        }
    }
}