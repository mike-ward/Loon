using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;
using Loon.Views.Content.Timelines;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileTimeline : UserControl
    {
        public static readonly string UserTimelineName = "UserTimeline";

        public UserProfileTimeline()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override async void OnDataContextChanged(EventArgs e)
        {
            if (this.FindControl<TimelineView>(UserTimelineName) is TimelineView tlv &&
                tlv.DataContext is UserProfileTimelineViewModel vm)
            {
                vm.StatusCollection.Clear();

                if (DataContext is User user)
                {
                    await Task.Delay(500).ConfigureAwait(true);
                    var statuses = await vm.GetUserTimeline(user.ScreenName!).ConfigureAwait(true);
                    vm.StatusCollection.AddRange(statuses.OrderByDescending(status => status.OriginatingStatus.CreatedDate));
                }
            }

            base.OnDataContextChanged(e);
        }
    }
}