using System.Threading.Tasks;

namespace Loon.Models
{
    internal static class TruncateStatusCollectionTask
    {
        public static ValueTask Execute(Timeline timeline)
        {
            const int maxNumberOfStatuses = 500;

            if (timeline.StatusCollection.Count > maxNumberOfStatuses)
            {
                timeline.StatusCollection.RemoveRange(maxNumberOfStatuses, timeline.StatusCollection.Count - maxNumberOfStatuses);
            }

            return default;
        }
    }
}