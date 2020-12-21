using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsTheme : UserControl
    {
        public SettingsTheme()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}