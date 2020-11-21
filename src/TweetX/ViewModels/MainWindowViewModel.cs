using Avalonia;
using TweetX.Interfaces;

namespace TweetX.ViewModels
{
    internal class MainWindowViewModel
    {
        private ISettings Settings { get; }

        public MainWindowViewModel(ISettings settings)
        {
            Settings = settings;
        }

        public void Initialize(IWindow window)
        {
            Settings.Load();
            SetWindowLocation(window);
        }

        public void Close(IWindow window)
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