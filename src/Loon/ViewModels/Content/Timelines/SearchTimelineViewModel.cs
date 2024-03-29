﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    internal sealed class SearchTimelineViewModel : NotifyPropertyChanged
    {
        private readonly ITwitterService              twitterService;
        public           IAvaloniaList<TwitterStatus> StatusCollection { get; } = new AvaloniaList<TwitterStatus>();
        private          bool                         isSearching;

        public bool IsSearching
        {
            get => isSearching;
            set => SetProperty(ref isSearching, value);
        }

        public SearchTimelineViewModel(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
        }

        public ValueTask<IEnumerable<TwitterStatus>> UpdateSearchTimeline(string screenName)
        {
            return twitterService.TwitterApi.GetUserTimeline(screenName);
        }

        public async ValueTask OnSearch(string search)
        {
            StatusCollection.Clear();
            IsSearching = true;
            var tweets = await twitterService.TwitterApi.Search(search).ConfigureAwait(true);
            IsSearching = false;
            StatusCollection.AddRange(tweets.Statuses);
        }
    }
}