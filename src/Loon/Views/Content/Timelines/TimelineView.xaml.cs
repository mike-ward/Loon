using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    public class TimelineView : UserControl
    {
        public static readonly string ScrollViewerName = "ScrollViewer";
        public static readonly string ItemsControlName = "ItemsControl";

        public TimelineView()
        {
            AvaloniaXamlLoader.Load(this);
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

        // ReSharper disable once UnusedParameter.Local
        private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer &&
                DataContext is HomeTimelineViewModel homeTimelineViewModel)
            {
                var timeline = homeTimelineViewModel.HomeTimeline;
                timeline.IsScrolled = scrollViewer.Offset.Y != 0;

                if (!timeline.IsScrolled && timeline.PendingStatusesAvailable)
                {
                    timeline.StatusCollection.InsertRange(0, timeline.PendingStatusCollection);
                    timeline.PendingStatusCollection.Clear();
                    timeline.PendingStatusesAvailable = false;
                }
            }
        }

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            // Scroll faster
            if ((sender as Control)?.Parent is ScrollViewer scrollViewer)
            {
                var offset = new Vector(0, 100); // 100 feels about right scroll-wise

                if (e.Delta.Y < 0)
                {
                    scrollViewer.Offset += offset;
                }
                else
                {
                    scrollViewer.Offset -= offset;
                }
            }
        }
    }
}