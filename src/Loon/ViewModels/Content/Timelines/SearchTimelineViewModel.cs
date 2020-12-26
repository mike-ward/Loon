using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    internal class SearchTimelineViewModel : NotifyPropertyChanged
    {
        private bool isSearching;
        private ITwitterService TwitterService { get; }
        public IAvaloniaList<TwitterStatus> StatusCollection { get; private set; } = new AvaloniaList<TwitterStatus>();
        public bool IsSearching { get => isSearching; set => SetProperty(ref isSearching, value); }

        public SearchTimelineViewModel(ITwitterService twitterService)
        {
            TwitterService = twitterService;
        }

        public ValueTask<IEnumerable<TwitterStatus>> UpdateSearchTimeline(string screenName)
        {
            return TwitterService.GetUserTimeline(screenName);
        }

        public async ValueTask OnSearch(string search)
        {
            StatusCollection.Clear();
            IsSearching = true;
            var tweets = await TwitterService.Search(search).ConfigureAwait(true);
            IsSearching = false;
            StatusCollection.AddRange(tweets.Statuses);
        }
    }
}