using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Loon.Views.Content.Timelines.TweetItem;

namespace Loon.Views.Content.Timelines
{
    public class TimelineView : UserControl
    {
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
                this.FindDescendantOfType<ScrollViewer>()?.ScrollToHome();
            }
        }
    }
}