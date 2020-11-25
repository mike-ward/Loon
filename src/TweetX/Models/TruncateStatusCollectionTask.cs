using System.Threading.Tasks;

namespace TweetX.Models
{
    public static class TruncateStatusCollectionTask
    {
        public static ValueTask Execute(Timeline timeline)
        {
            const int maxNumberOfStatuses = 500;

            while (timeline.StatusCollection.Count > maxNumberOfStatuses)
            {
                timeline.StatusCollection.RemoveAt(timeline.StatusCollection.Count - 1);
            }

            return default;
        }
    }
}