using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Collections;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    internal class UserProfileTimelineViewModel
    {
        private readonly ITwitterService              twitterService;
        public           IAvaloniaList<TwitterStatus> StatusCollection { get; } = new AvaloniaList<TwitterStatus>();

        public UserProfileTimelineViewModel(ITwitterService twitterService)
        {
            this.twitterService = twitterService;
        }

        public ValueTask<IEnumerable<TwitterStatus>> GetUserTimeline(string screenName)
        {
            return twitterService.TwitterApi.GetUserTimeline(screenName);
        }
    }
}