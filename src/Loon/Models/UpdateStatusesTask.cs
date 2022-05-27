using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Models;

namespace Loon.Models
{
    internal static class UpdateStatusesTask
    {
        public static ValueTask Execute(IEnumerable<TwitterStatus> statuses, Timeline timeline)
        {
            var current = new HashSet<TwitterStatus>(timeline.StatusCollection); // faster lookups
            var latest  = new List<TwitterStatus>();

            foreach (var status in statuses.OrderByDescending(status => status.CreatedDate))
            {
                if (current.TryGetValue(status, out var statusToUpdate))
                {
                    statusToUpdate.UpdateFromStatus(status);
                }
                else if (timeline.AlreadyAdded.Add(status.Id))
                {
                    status.UpdateAboutMeProperties(timeline.Settings.ScreenName);

                    if (timeline.IsScrolled)
                    {
                        timeline.PendingStatusCollection.Add(status);
                        timeline.PendingStatusesAvailable = true;
                    }
                    else
                    {
                        latest.Add(status);
                    }
                }
            }

            TruncatePendingStatuses(timeline);
            timeline.StatusCollection.InsertRange(0, latest.Take(Constants.MaxNumberOfStatuses));
            return default;
        }

        private static void TruncatePendingStatuses(Timeline timeline)
        {
            var truncateCount = timeline.PendingStatusCollection.Count - Constants.MaxNumberOfStatuses;
            if (truncateCount <= 0) return;

            foreach (var item in timeline.PendingStatusCollection.TakeLast(truncateCount))
            {
                timeline.PendingStatusCollection.Remove(item);
            }
        }
    }
}