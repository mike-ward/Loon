using Avalonia;
using Loon.Interfaces;

namespace Loon.ViewModels
{
    internal class MainWindowViewModel
    {
        public ISettings Settings { get; }

        public MainWindowViewModel(ITwitterService twitterService, ISettings settings)
        {
            Settings = settings;

            Settings.PropertyChanged += delegate
            {
                twitterService.AuthenticationTokens(
                    Settings.AccessToken,
                    Settings.AccessTokenSecret);
            };
        }

        public void Load(IWindow window)
        {
            Settings.Load();
            SetWindowLocation(window);
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
    }
}