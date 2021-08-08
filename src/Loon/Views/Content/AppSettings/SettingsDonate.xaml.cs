using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    internal class SettingsDonate : UserControl
    {
        public SettingsDonate()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnDonate(string par)
        {
            Services.OpenUrlService.Open(par);
        }
    }
}