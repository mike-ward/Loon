using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Loon.Services;
using Loon.Views.Content.Controls;
using Twitter.Models;

namespace Loon.ViewModels.Content.Write
{
    internal class WriteViewModel : NotifyPropertyChanged
    {
        private readonly ISettings       settings;
        private readonly ITwitterService twitterService;

        private static readonly string tweetButtonTextConst    = App.GetString("tweet-button-text");
        private static readonly string tweetingButtonTextConst = App.GetString("tweeting-button-text");

        private TwitterStatus? twitterStatus;

        public TwitterStatus? Me
        {
            get => twitterStatus;
            set => SetProperty(ref twitterStatus, value);
        }

        private TwitterStatus? replyTo;

        public TwitterStatus? ReplyTo
        {
            get => replyTo;
            set => SetProperty(ref replyTo, value);
        }

        private string tweetText = string.Empty;

        public string TweetText
        {
            get => tweetText;
            set => SetProperty(ref tweetText, value);
        }

        private string tweetButtonText = tweetButtonTextConst;

        public string TweetButtonText
        {
            get => tweetButtonText;
            set => SetProperty(ref tweetButtonText, value);
        }

        private bool isTweeting;

        public bool IsTweeting
        {
            get => isTweeting;
            set => SetProperty(ref isTweeting, value);
        }

        public WriteViewModel(ISettings settings, ITwitterService twitterService)
        {
            this.settings       = settings;
            this.twitterService = twitterService;
            PubSubs.OpenWriteTab.Subscribe(twitterStatusArg => ReplyTo = twitterStatusArg);
            settings.PropertyChanged += Settings_PropertyChanged;
        }

        public async ValueTask OnTweet()
        {
            if (TweetText.Length == 0) { return; }

            IsTweeting      = true;
            TweetButtonText = tweetingButtonTextConst;

            try
            {
                var status = await twitterService.TwitterApi.UpdateStatus(TweetText, ReplyTo?.Id, null, Array.Empty<string>()).ConfigureAwait(true);
                PubSubs.AddStatus.Publish(status);
                PubSubs.OpenPreviousTab.Publish(Unit._);
                Reset();
            }
            catch (WebException ex)
            {
                var stream = ex.Response?.GetResponseStream();
                if (stream is null) { return; }

                using var reader  = new StreamReader(stream);
                var       message = await reader.ReadToEndAsync().ConfigureAwait(true);

                await MessageBox
                    .Show(message, MessageBox.MessageBoxButtons.Ok)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                await MessageBox
                    .Show(ex.Message, MessageBox.MessageBoxButtons.Ok)
                    .ConfigureAwait(true);
            }
        }

        private void Reset()
        {
            ReplyTo         = null;
            IsTweeting      = false;
            TweetText       = string.Empty;
            TweetButtonText = tweetButtonText;
        }

        private async void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(ISettings.ScreenName)) && settings.ScreenName is { } screenName)
            {
                try
                {
                    var statuses = await twitterService.TwitterApi.GetUserTimeline(screenName).ConfigureAwait(true);
                    Me = statuses?.First();
                }
                catch (HttpRequestException)
                {
                    // eat it
                }
            }
        }
    }
}