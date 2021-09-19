using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.Models;

namespace Loon.Models
{
    internal static class UpdateStatusesTask
    {
        public static ValueTask Execute(IEnumerable<TwitterStatus> statuses, Timeline timeline)
        {
            // Build a hashset for faster lookups.
            var hashSet = new HashSet<TwitterStatus>(timeline.StatusCollection);
            var staged  = new List<TwitterStatus>();

            foreach (var status in statuses.OrderBy(status => status.CreatedDate))
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
                        if (timeline.StatusCollection.Count == 0)
                        {
                            // Remove when ItemsRepeater bug is fixed.
                            timeline.StatusCollection.Insert(0, new TwitterStatus());
                        }

                        staged.Insert(0, clonedStatus);
                    }
                }
            }

            // Insert at zero when ItemsRepeater bug is fixed.
            timeline.StatusCollection.InsertRange(1, staged);
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