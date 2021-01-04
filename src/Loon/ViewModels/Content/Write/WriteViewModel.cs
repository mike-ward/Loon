using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly ISettings settings;
        private readonly ITwitterService twitterService;

        private readonly string tweetButtonText = App.GetString("tweet-button-text");
        private readonly string tweetingButtonText = App.GetString("tweeting-button-text");

        public TwitterStatus? Me { get => Getter(default(TwitterStatus)); set => Setter(value); }
        public TwitterStatus? ReplyTo { get => Getter(default(TwitterStatus)); set => Setter(value); }
        public string TweetText { get => Getter(string.Empty); set => Setter(value); }
        public string TweetButtonText { get => Getter(tweetButtonText); set => Setter(value); }
        public bool IsTweeting { get => Getter(false); set => Setter(value); }

        public WriteViewModel(ISettings settings, ITwitterService twitterService)
        {
            this.settings = settings;
            this.twitterService = twitterService;
            this.settings.PropertyChanged += Settings_PropertyChanged;
            PubSubService.AddSubscriber(PubSubService.OpenWriteTabMessage, OpenWriteTabHandler);
        }

        private async void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(ISettings.ScreenName)) && settings.ScreenName is string screenName)
            {
                var statuses = await twitterService.GetUserTimeline(screenName).ConfigureAwait(true);
                Me = statuses?.First();
            }
        }

        private void OpenWriteTabHandler(object? payload)
        {
            ReplyTo = payload as TwitterStatus;
        }

        public async ValueTask OnTweet()
        {
            if (TweetText.Length == 0) { return; }
            IsTweeting = true;
            TweetButtonText = tweetingButtonText;

            try
            {
                var status = await twitterService.UpdateStatus(TweetText, ReplyTo?.Id, null, Array.Empty<string>()).ConfigureAwait(true);
                PubSubService.Publish(PubSubService.AddStatusMessage, status);
                PubSubService.Publish(PubSubService.OpenPreviousTabMessage, null);
                Reset();
            }
            catch (WebException ex)
            {
                var stream = ex.Response?.GetResponseStream();
                if (stream is null) { return; }
                using var reader = new StreamReader(stream);
                var message = await reader.ReadToEndAsync();

                await MessageBox
                    .Show(App.MainWindow, message, App.GetString("title"), MessageBox.MessageBoxButtons.Ok)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                await MessageBox
                    .Show(App.MainWindow, ex.Message, App.GetString("title"), MessageBox.MessageBoxButtons.Ok)
                    .ConfigureAwait(true);
            }
            finally
            {
                Reset();
            }
        }

        public void Reset()
        {
            ReplyTo = null;
            IsTweeting = false;
            TweetText = string.Empty;
            TweetButtonText = tweetButtonText;
        }
    }
}