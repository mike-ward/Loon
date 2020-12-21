using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsSignOut : UserControl
    {
        public SettingsSignOut()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Signout()
        {
            if (DataContext is ISettings settings)
            {
                settings.AccessToken = null;
                settings.AccessTokenSecret = null;
            }
        }
    }
}