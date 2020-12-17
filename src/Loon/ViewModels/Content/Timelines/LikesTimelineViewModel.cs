using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loon.Interfaces;
using Loon.Models;

namespace Loon.ViewModels.Content.Timelines
{
    public class LikesTimelineViewModel : NotifyPropertyChanged
    {
        private Timeline? timeline;
        private ITwitterService TwitterService { get; }

        public Timeline? Timeline { get => timeline; set => SetProperty(ref timeline, value); }

        public LikesTimelineViewModel(ISettings settings, ITwitterService twitterService)
        {
            TwitterService = twitterService;
            var name = App.GetString("tab-likes-name");
            Timeline = new Timeline(name: name, intervalInMinutes: 20, updateTasks: Tasks(), settings: settings);
        }

        private IEnumerable<Func<Timeline, ValueTask>> Tasks()
        {
            return new Func<Timeline, ValueTask>[]
            {
                timeline => GetAndUpdateStatusesAsync(timeline),
                timeline => TruncateStatusCollectionTask.Execute(timeline),
                timeline => UpdateTimeStampsTask.Execute(timeline),
            };
        }

        private async ValueTask GetAndUpdateStatusesAsync(Timeline timeline)
        {
            var statuses = await TwitterService.GetFavoritesTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses, timeline).ConfigureAwait(true);
        }
    }
}