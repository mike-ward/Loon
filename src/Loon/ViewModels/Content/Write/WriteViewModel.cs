using System.ComponentModel;
using System.Linq;
using Loon.Commands;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Write
{
    internal class WriteViewModel : NotifyPropertyChanged
    {
        public ISettings Settings { get; }
        public ITwitterService TwitterService { get; }

        private TwitterStatus? me;
        public TwitterStatus? Me { get => me; set => SetProperty(ref me, value); }

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
            TabGoBackCommand.Command.Execute(null);
        }
    }
}