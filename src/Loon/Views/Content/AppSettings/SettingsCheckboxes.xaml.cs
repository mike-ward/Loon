using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsCheckboxes : UserControl
    {
        public SettingsCheckboxes()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
