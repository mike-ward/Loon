using System.Linq;
using System.Threading.Tasks;
using Loon.Services;

namespace Loon.Models
{
    internal static class FlowContentTask
    {
        public static ValueTask Execute(Timeline timeline)
        {
            foreach (var status in timeline.StatusCollection)
            {
                if (status.FlowContent is null)
                {
                    status.FlowContent = FlowContentService.FlowContentNodes(status).ToArray();
                }
            }

            return default;
        }
    }
}