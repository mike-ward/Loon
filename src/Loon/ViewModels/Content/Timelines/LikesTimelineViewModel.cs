using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    internal class LikesTimelineViewModel
    {
        private readonly Timeline        likesTimeline;
        private readonly ITwitterService twitterService;

        public IAvaloniaList<TwitterStatus> StatusCollection => likesTimeline.StatusCollection;

        public LikesTimelineViewModel(ISettings settings, ITwitterService twitterService)
        {
            this.twitterService = twitterService;
            var name = App.GetString("tab-likes-name");
            likesTimeline = new Timeline(name: name, intervalInMinutes: 20, updateTasks: Tasks(), settings: settings);
        }

        private IEnumerable<Func<Timeline, ValueTask>> Tasks()
        {
            return new Func<Timeline, ValueTask>[] {
                timeline => GetAndUpdateStatusesAsync(timeline),
                timeline => TruncateStatusCollectionTask.Execute(timeline),
                timeline => FlowContentTask.Execute(timeline),
                timeline => UpdateTimeStampsTask.Execute(timeline)
            };
        }

        private async ValueTask GetAndUpdateStatusesAsync(Timeline timeline)
        {
            var statuses = await twitterService.GetFavoritesTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses, timeline).ConfigureAwait(true);
        }
    }
}