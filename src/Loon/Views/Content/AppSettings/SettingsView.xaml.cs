using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Interfaces;

namespace Loon.Views.Content.AppSettings
{
    internal class SettingsView : UserControl
    {
        public SettingsView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<ISettings>();
        }
    }
}