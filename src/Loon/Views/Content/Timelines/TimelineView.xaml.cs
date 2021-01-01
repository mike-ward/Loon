using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Views.Content.Controls.TweetItem;

namespace Loon.Views.Content.Timelines
{
    public class TimelineView : UserControl
    {
        public static readonly string ScrollViewerName = "ScrollViewer";

        public TimelineView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnElementClearing(object? sender, ItemsRepeaterElementClearingEventArgs e)
        {
            if (e.Element is TweetItemView ti)
            {
                ti.Clearing();
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.GetCurrentPoint(relativeTo: null).Properties.IsRightButtonPressed)
            {
                e.Handled = true;
                this.FindControl<ScrollViewer>(ScrollViewerName)?.ScrollToHome();
            }
        }
    }
}