using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    public class HomeTimelineViewModel
    {
        private const int mentionsInterval = 60;
        private int mentionsCounter = mentionsInterval;

        private Timeline Timeline { get; }
        private ITwitterService TwitterService { get; }

        public const string AddStatusMessage = "add-status-message";
        public IAvaloniaList<TwitterStatus> StatusCollection { get => Timeline.StatusCollection; }

        public HomeTimelineViewModel(ISettings settings, ITwitterService twitterService, IPubSubService pubSub)
        {
            TwitterService = twitterService;
            var name = App.GetString("tab-home-name");
            Timeline = new Timeline(name: name, intervalInMinutes: 1.1, updateTasks: Tasks(), settings: settings);
            pubSub.PubSubRaised += (s, e) => AddStatusHandler(e);
        }

        private IEnumerable<Func<Timeline, ValueTask>> Tasks()
        {
            return new Func<Timeline, ValueTask>[]
            {
                timeline => GetAndUpdateStatusesAsync(timeline),
                timeline => TruncateStatusCollectionTask.Execute(timeline),
                timeline => UpdateTimeStampsTask.Execute(timeline),
                timeline => CollectTask.Execute(timeline),
            };
        }

        private async ValueTask GetAndUpdateStatusesAsync(Timeline timeline)
        {
            var mentions = await GetMentionsAsync().ConfigureAwait(true);
            var statuses = await TwitterService.GetHomeTimeline().ConfigureAwait(true);
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
                    return await TwitterService.GetMentionsTimeline().ConfigureAwait(true);
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

        private void AddStatusHandler(PubSubEventArgs e)
        {
            if (e.Message.IsEqualTo(AddStatusMessage) && e.Payload is TwitterStatus status)
            {
                UpdateStatuses.Execute(new[] { status }, Timeline);
            }
        }
    }
}