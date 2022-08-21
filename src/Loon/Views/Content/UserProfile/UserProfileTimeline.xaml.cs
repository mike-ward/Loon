using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Services;
using Loon.ViewModels.Content.Timelines;
using Loon.Views.Content.Controls;
using Loon.Views.Content.Timelines;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public sealed class UserProfileTimeline : UserControl
    {
        public static readonly string UserTimelineName = "UserTimeline";

        public UserProfileTimeline()
        {
            AvaloniaXamlLoader.Load(this);
            this.FindControl<TimelineView>(UserTimelineName)!.DataContext = App.ServiceProvider.GetService<UserProfileTimelineViewModel>();
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        protected override async void OnDataContextChanged(EventArgs e)
        {
            try
            {
                if (this.FindControl<TimelineView>(UserTimelineName) is { DataContext: UserProfileTimelineViewModel vm })
                {
                    vm.StatusCollection.Clear();

                    if (DataContext is User user)
                    {
                        await Task.Delay(500).ConfigureAwait(true);
                        try
                        {
                            var statuses = await vm.GetUserTimeline(user.ScreenName!).ConfigureAwait(true);
                            vm.StatusCollection.AddRange(statuses.OrderByDescending(status => status.OriginatingStatus.CreatedDate));
                        }
                        catch (Exception ex)
                        {
                            await MessageBox.Show(ex.Message, MessageBox.MessageBoxButtons.Ok);
                        }
                    }
                }

                base.OnDataContextChanged(e);
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}