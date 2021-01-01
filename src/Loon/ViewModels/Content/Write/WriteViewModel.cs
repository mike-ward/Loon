using System.ComponentModel;
using System.Linq;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Write
{
    internal class WriteViewModel : NotifyPropertyChanged
    {
        private ISettings Settings { get; }
        private ITwitterService TwitterService { get; }

        public TwitterStatus? Me { get => Getter<TwitterStatus?>(); set => Setter(value); }
        public TwitterStatus? ReplyTo { get => Getter<TwitterStatus?>(); set => Setter(value); }

        public WriteViewModel(ISettings settings, ITwitterService twitterService)
        {
            Settings = settings;
            TwitterService = twitterService;

            Settings.PropertyChanged += Settings_PropertyChanged;
        }

        private async void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(ISettings.ScreenName)) && Settings.ScreenName is string screenName)
            {
                var statuses = await TwitterService.GetUserTimeline(screenName).ConfigureAwait(true);
                Me = statuses?.First();
            }
        }

        public void OnTweet()
        {
            App.Commands.TabGoBack.Execute(null);
        }

        public void Reset()
        {
            ReplyTo = null;
        }
    }
}