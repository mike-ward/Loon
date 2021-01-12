﻿using System;
using System.Threading.Tasks;
using Loon.Extensions;
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

        private string? pin;
        public string? Pin { get => pin; set => SetProperty(ref pin, value); }
        private bool secondPage;
        public bool SecondPage { get => secondPage; set => SetProperty(ref secondPage, value); }

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
            if (Pin.IsNullOrWhiteSpace()) { throw new InvalidOperationException("Pin is null"); }

            var access = await twitterService.AuthenticateWithPinAsync(requestToken, Pin!).ConfigureAwait(false);
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
                    .Show(App.GetString("pin-error"), MessageBox.MessageBoxButtons.Ok)
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