using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Loon.Models;

namespace Loon.Views.Content.AppSettings
{
    public sealed class SettingsZoom : UserControl
    {
        public SettingsZoom()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnCheckedZoom100(object? sender, RoutedEventArgs e)
        {
            if (DataContext is Settings settings) settings.Zoom = Settings.Zoom100Percent;
        }

        private void OnCheckedZoom150(object? sender, RoutedEventArgs e)
        {
            if (DataContext is Settings settings) settings.Zoom = Settings.Zoom150Percent;
        }
        
        private void OnCheckedZoom200(object? sender, RoutedEventArgs e)
        {
            if (DataContext is Settings settings) settings.Zoom = Settings.Zoom200Percent;
        }
    }
}