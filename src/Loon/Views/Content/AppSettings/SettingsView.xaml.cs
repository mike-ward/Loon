using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;

namespace Loon.Views.Content.AppSettings
{
    public sealed class SettingsView : UserControl
    {
        public SettingsView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<ISettings>();
        }
    }
}