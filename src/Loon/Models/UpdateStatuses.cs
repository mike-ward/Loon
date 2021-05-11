using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.Models;

namespace Loon.Models
{
    public static class UpdateStatuses
    {
        public static ValueTask Execute(IEnumerable<TwitterStatus> statuses, Timeline timeline)
        {
            // Build a hashset for faster lookups.
            var hashSet = new HashSet<TwitterStatus>(timeline.StatusCollection);

            foreach (var status in statuses.OrderBy(status => status.OriginatingStatus.CreatedDate))
            {
                if (hashSet.TryGetValue(status, out var statusToUpdate))
                {
                    statusToUpdate.UpdateFromStatus(status);
                }
                else if (!timeline.AlreadyAdded.Contains(status.Id))
                {
                    var clonedStatus = Clone(status);
                    timeline.AlreadyAdded.Add(clonedStatus.Id);
                    clonedStatus.UpdateAboutMeProperties(timeline.Settings.ScreenName);

                    if (timeline.IsScrolled)
                    {
                        timeline.PendingStatusCollection.Add(clonedStatus);
                        timeline.PendingStatusesAvailable = true;
                    }
                    else
                    {
                        if (timeline.StatusCollection.Count == 0) { timeline.StatusCollection.Insert(0, new TwitterStatus()); }

                        // Insert at zero when ItemsRepeater bug is fixed.
                        timeline.StatusCollection.Insert(1, clonedStatus);
                    }
                }
            }

            return default;
        }

        private static TwitterStatus Clone(TwitterStatus twitterStatus)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(twitterStatus);
            var span  = new ReadOnlySpan<byte>(bytes);
            return JsonSerializer.Deserialize<TwitterStatus>(span)!;
        }
    }
}