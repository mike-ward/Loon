using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;
using Loon.Views.Content.Timelines;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileTimeline : UserControl
    {
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
            base.OnDataContextChanged(e);

            if (this.FindControl<TimelineView>("UserTimeline") is TimelineView tlv &&
                tlv.DataContext is UserProfileTimelineViewModel vm)
            {
                vm.StatusCollection.Clear();

                if (DataContext is User user)
                {
                    var statuses = await vm.GetUserTimeline(user.ScreenName!).ConfigureAwait(true);
                    vm.StatusCollection.AddRange(statuses.OrderByDescending(status => status.OriginatingStatus.CreatedDate));
                }
            }
        }
    }
}