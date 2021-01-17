using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Loon.Models;
using Loon.Services;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    public class HomeTimelineViewModel
    {
        private const int mentionsInterval = 60;
        private int mentionsCounter = mentionsInterval;

        private readonly Timeline timeline;
        private readonly ITwitterService titterService;

        public IAvaloniaList<TwitterStatus> StatusCollection { get => timeline.StatusCollection; }

        public HomeTimelineViewModel(ISettings settings, ITwitterService twitterService)
        {
            titterService = twitterService;
            var name = App.GetString("tab-home-name");
            timeline = new Timeline(name: name, intervalInMinutes: 1.1, updateTasks: Tasks(), settings: settings);
            PubSubs.AddStatus.Subscribe(status => UpdateStatuses.Execute(new[] { status }, timeline).ConfigureAwait(false));
        }

        private IEnumerable<Func<Timeline, ValueTask>> Tasks()
        {
            return new Func<Timeline, ValueTask>[]
            {
                timeline => GetAndUpdateStatusesAsync(timeline),
                timeline => TruncateStatusCollectionTask.Execute(timeline),
                timeline => UpdateTimeStampsTask.Execute(timeline),
                timeline => FlowContentTask.Execute(timeline),
                timeline => CollectTask.Execute(timeline),
            };
        }

        private async ValueTask GetAndUpdateStatusesAsync(Timeline timeline)
        {
            var mentions = await GetMentionsAsync().ConfigureAwait(true);
            var statuses = await titterService.GetHomeTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses.Concat(mentions), timeline).ConfigureAwait(true);
        }

        private async ValueTask<IEnumerable<TwitterStatus>> GetMentionsAsync()
        {
            try
            {
                // Twitter limits getting mentions to 100,000 per day per Application.
                // Application in this case means all running Tweetz clients. Once
                // an hour allows everybody get mentions albeit not in a timely manner.
                if (mentionsCounter++ >= mentionsInterval)
                {
                    mentionsCounter = 0;
                    return await titterService.GetMentionsTimeline().ConfigureAwait(true);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    // Probably hit the daily limit
                    // Alerting the user does no good in this instance (IMO)
                    return Enumerable.Empty<TwitterStatus>();
                }
                throw;
            }
            return Enumerable.Empty<TwitterStatus>();
        }
    }
}