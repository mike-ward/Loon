using Jab;
using Loon.Commands;
using Loon.Interfaces;
using Loon.Models;
using Loon.ViewModels;
using Loon.ViewModels.Content;
using Loon.ViewModels.Content.Timelines;
using Loon.ViewModels.Content.UserProfile;
using Loon.ViewModels.Content.Write;
using Loon.Views;

namespace Loon.Services
{
    [ServiceProvider]
    [Transient(typeof(MainWindow))]
    [Transient(typeof(AppCommands))]
    [Transient(typeof(MainWindowViewModel))]
    [Transient(typeof(GetPinViewModel))]
    [Transient(typeof(HomeTimelineViewModel))]
    [Transient(typeof(LikesTimelineViewModel))]
    [Transient(typeof(UserProfileTimelineViewModel))]
    [Transient(typeof(SearchTimelineViewModel))]
    [Transient(typeof(WriteViewModel))]
    [Transient(typeof(UserProfileViewModel))]
    [Singleton(typeof(ISettings), typeof(Settings))]
    [Singleton(typeof(ITwitterService), typeof(TwitterService))]
    public partial class JabServiceProvider { }

    internal static class Bootstrapper
    {
        public static JabServiceProvider ServiceProvider { get; } = new();
    }
}