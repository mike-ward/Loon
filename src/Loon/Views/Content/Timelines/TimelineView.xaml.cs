using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;
using Loon.Views.Content.Controls.TweetItem;

namespace Loon.Views.Content.Timelines
{
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    internal class TimelineView : UserControl
    {
        public static readonly string ScrollViewerName  = "ScrollViewer";
        public static readonly string ItemsRepeaterName = "ItemsRepeater";

        public TimelineView()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<ItemsRepeater>(ItemsRepeaterName).ElementClearing += (_, args) =>
            {
                var tweetItemView = (TweetItemView)args.Element;
                tweetItemView.CancellationTokenSource.Cancel();
            };
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton == MouseButton.Right)
            {
                e.Handled = true;
                this.FindControl<ScrollViewer>(ScrollViewerName)?.ScrollToHome();
            }

            base.OnPointerReleased(e);
        }

        private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer &&
                DataContext is HomeTimelineViewModel homeTimelineViewModel)
            {
                var timeline = homeTimelineViewModel.HomeTimeline;
                timeline.IsScrolled = scrollViewer.Offset.Y != 0;

                if (!timeline.IsScrolled && timeline.PendingStatusesAvailable)
                {
                    timeline.StatusCollection.InsertRange(1, timeline.PendingStatusCollection);
                    timeline.PendingStatusCollection.Clear();
                    timeline.PendingStatusesAvailable = false;
                }
            }
        }
    }
}