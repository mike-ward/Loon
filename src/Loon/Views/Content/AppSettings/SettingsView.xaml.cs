using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Interfaces;
using Loon.Services;

namespace Loon.Views.Content.AppSettings
{
    internal class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = Bootstrapper.ServiceProvider.GetService<ISettings>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}