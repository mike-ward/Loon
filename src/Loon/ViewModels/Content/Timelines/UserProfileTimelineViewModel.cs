using Avalonia.Collections;
using Loon.Models;
using Twitter.Models;

namespace Loon.ViewModels.Content.Timelines
{
    public class UserProfileTimelineViewModel : NotifyPropertyChanged
    {
        private string? screenName;

        public string? ScreenName { get => screenName; set => SetProperty(ref screenName, value); }

        public AvaloniaList<TwitterStatus> StatusCollection { get; } = new();
    }
}