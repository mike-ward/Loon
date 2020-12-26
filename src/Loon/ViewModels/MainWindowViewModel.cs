using System;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels
{
    internal class MainWindowViewModel : NotifyPropertyChanged
    {
        private User? user;

        public ITwitterService TwitterService { get; }

        public User? User { get => user; set => SetProperty(ref user, value); }

        public ISettings Settings { get; }

        public MainWindowViewModel(ITwitterService twitterService, ISettings settings)
        {
            TwitterService = twitterService;
            Settings = settings;

            Settings.PropertyChanged += (_, e) =>
            {
                TwitterService.AuthenticationTokens(
                    Settings.AccessToken,
                    Settings.AccessTokenSecret);

                UpdateTheme(e.PropertyName);
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

        public void SetUser(string screenName)
        {
            if (screenName is null)
            {
                User = null;
            }
            else
            {
                var task = TwitterService.UserInfo(screenName).AsTask();
                task.Wait();
                User = task.Result;
            }
        }

        private void UpdateTheme(string? propertyName)
        {
            if (propertyName.IsEqualTo(nameof(ISettings.UseLightTheme)))
            {
                var styles = new StyleInclude(new Uri("resm:Styles"))
                {
                    Source = Settings.UseLightTheme
                        ? new Uri("avares://Avalonia.Themes.Default/Accents/BaseLight.xaml")
                        : new Uri("avares://Avalonia.Themes.Default/Accents/BaseDark.xaml")
                };

                App.Current.Styles[1] = styles;
            }
        }
    }
}