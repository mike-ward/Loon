using System;
using System.Threading.Tasks;
using Loon.Interfaces;
using Loon.Models;
using Loon.Views;
using Twitter.Services;

namespace Loon.ViewModels.Content
{
    internal class GetPinViewModel : NotifyPropertyChanged
    {
        private OAuthTokens? requestToken;

        public string? Pin { get => GetProp<string>(); set => SetProp(value); }
        public bool SecondPage { get => GetProp<bool>(); set => SetProp(value); }

        public ITwitterService Twitter { get; }
        public ISettings Settings { get; }

        public GetPinViewModel(ITwitterService twitter, ISettings settings)
        {
            Twitter = twitter;
            Settings = settings;
        }

        public async ValueTask GetPin()
        {
            requestToken = await Twitter.GetPin().ConfigureAwait(false);
            SecondPage = true;
        }

        public async ValueTask SignIn()
        {
            if (requestToken is null) { throw new InvalidOperationException("requestToken is null"); }
            if (string.IsNullOrWhiteSpace(Pin)) { throw new InvalidOperationException("Pin is null"); }

            var access = await Twitter.AuthenticateWithPinAsync(requestToken, Pin).ConfigureAwait(false);
            GoBack();

            if (access is not null)
            {
                Settings.AccessToken = access.OAuthToken;
                Settings.AccessTokenSecret = access.OAuthSecret;
                Settings.ScreenName = access.ScreenName;
                Settings.Save();
            }
            else
            {
                await MessageBox
                    .Show(App.MainWindow, App.GetString("pin-error"), App.GetString("title"), MessageBox.MessageBoxButtons.Ok)
                    .ConfigureAwait(false);
            }
        }

        public void GoBack()
        {
            Pin = null;
            SecondPage = false;
        }
    }
}