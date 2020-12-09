using System.Threading.Tasks;

namespace Loon.Models
{
    public static class UpdateTimeStampsTask
    {
        public static ValueTask Execute(Timeline timeline)
        {
            foreach (var status in timeline.StatusCollection)
            {
                status.InvokeUpdateTimeStamp();
            }

            return default;
        }
    }
}