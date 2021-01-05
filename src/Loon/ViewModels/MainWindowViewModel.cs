using System.ComponentModel;
using Avalonia;
using Loon.Extensions;
using Loon.Interfaces;

namespace Loon.ViewModels
{
    internal class MainWindowViewModel
    {
        private readonly ITwitterService twitterService;

        public ISettings Settings { get; set; }

        public MainWindowViewModel(ISettings settings, ITwitterService twitterService)
        {
            Settings = settings;
            this.twitterService = twitterService;
            settings.PropertyChanged += OnSettingsUpdated;
        }

        public void Load(IWindow window)
        {
            Settings.Load();
            SetWindowLocation(window);
            window.Closing += delegate { Save(window); };
        }

        public void Save(IWindow window)
        {
            UpdateWindowLocation(window);
            Settings.Save();
        }

        private void SetWindowLocation(IWindow window)
        {
            window.Width = Settings.Location.Width;
            window.Height = Settings.Location.Height;
            window.Position = new PixelPoint(Settings.Location.X, Settings.Location.Y);
        }

        private void UpdateWindowLocation(IWindow window)
        {
            Settings.Location.X = window.Position.X;
            Settings.Location.Y = window.Position.Y;
            Settings.Location.Width = window.Width;
            Settings.Location.Height = window.Height;
        }

        private void OnSettingsUpdated(object? _, PropertyChangedEventArgs e)
        {
            twitterService.AuthenticationTokens(
                Settings.AccessToken,
                Settings.AccessTokenSecret);

            if (e.PropertyName.IsEqualTo(nameof(ISettings.UseLightTheme)))
            {
                App.Commands.UpdateTheme.Execute(Settings.UseLightTheme);
            }
        }
    }
}