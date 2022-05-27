using System.Threading.Tasks;

namespace Loon.Models
{
    internal static class TruncateStatusCollectionTask
    {
        public static ValueTask Execute(Timeline timeline)
        {
            if (timeline.StatusCollection.Count > Constants.MaxNumberOfStatuses)
            {
                timeline.StatusCollection.RemoveRange(Constants.MaxNumberOfStatuses, timeline.StatusCollection.Count - Constants.MaxNumberOfStatuses);
            }

            return default;
        }
    }
}