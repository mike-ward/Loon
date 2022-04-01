using System.Collections.Generic;

namespace Twitter.Models
{
    public class TwitterStatusSortComparer : IComparer<TwitterStatus>
    {
        public int Compare(TwitterStatus? x, TwitterStatus? y)
        {
            // Descending order
            return y!.CreatedDate.CompareTo(x!.CreatedDate);
        }
    }
}