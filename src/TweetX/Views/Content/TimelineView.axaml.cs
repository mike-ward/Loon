using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TweetX.Views.Content.TweetItem;

namespace TweetX.Views.Content
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
    }
}