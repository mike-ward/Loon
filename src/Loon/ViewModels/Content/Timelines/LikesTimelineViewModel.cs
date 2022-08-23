using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Loon.Models;
using Loon.Services;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    internal sealed class LikesTimelineViewModel
    {
        private readonly Timeline        likesTimeline;
        private readonly ITwitterService twitterService;

        public IAvaloniaList<TwitterStatus> StatusCollection => likesTimeline.StatusCollection;

        public LikesTimelineViewModel(ISettings settings, ITwitterService twitterService)
        {
            this.twitterService = twitterService;
            var name = App.GetString("tab-likes-name");
            likesTimeline = new Timeline(name, 20, Tasks(), settings);
            PubSubs.UpdateLikesTimeline.Subscribe(UpdateAsync);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void UpdateAsync(Unit _)
        {
            try
            {
                await likesTimeline.UpdateAsync();
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private IEnumerable<UpdateTaskFunc> Tasks()
        {
            yield return GetAndUpdateStatusesAsync;
            yield return TruncateStatusCollectionTask.Execute;
            yield return UpdateTimeStampsTask.Execute;
        }

        private async ValueTask GetAndUpdateStatusesAsync(Timeline timeline)
        {
            var statuses = await twitterService.TwitterApi.FavoritesTimeline().ConfigureAwait(true);
            await UpdateStatusesTask.Execute(statuses, timeline).ConfigureAwait(true);
        }
    }
}