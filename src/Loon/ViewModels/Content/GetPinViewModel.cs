using System;
using System.Threading.Tasks;
using Loon.Interfaces;
using Loon.Models;
using Loon.Views.Content.Controls;
using Twitter.Services;

namespace Loon.ViewModels.Content
{
    internal class GetPinViewModel : NotifyPropertyChanged
    {
        private OAuthTokens? requestToken;
        private readonly ITwitterService twitterService;

        public string? Pin { get => Getter(default(string)); set => Setter(value); }
        public bool SecondPage { get => Getter(false); set => Setter(value); }

        public ISettings Settings { get; }

        public GetPinViewModel(ITwitterService twitterService, ISettings settings)
        {
            this.twitterService = twitterService;
            Settings = settings;
        }

        public async ValueTask GetPin()
        {
            requestToken = await twitterService.GetPin().ConfigureAwait(false);
            SecondPage = true;
        }

        public async ValueTask SignIn()
        {
            if (requestToken is null) { throw new InvalidOperationException("requestToken is null"); }
            if (string.IsNullOrWhiteSpace(Pin)) { throw new InvalidOperationException("Pin is null"); }

            var access = await twitterService.AuthenticateWithPinAsync(requestToken, Pin).ConfigureAwait(false);
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