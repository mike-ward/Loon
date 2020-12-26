using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    internal class UserProfileTimelineViewModel
    {
        private ITwitterService TwitterService { get; }
        public IAvaloniaList<TwitterStatus> StatusCollection { get; } = new AvaloniaList<TwitterStatus>();

        public UserProfileTimelineViewModel(ITwitterService twitterService)
        {
            TwitterService = twitterService;
        }

        public ValueTask<IEnumerable<TwitterStatus>> GetUserTimeline(string screenName)
        {
            return TwitterService.GetUserTimeline(screenName);
        }
    }
}