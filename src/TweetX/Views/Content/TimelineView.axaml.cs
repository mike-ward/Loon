using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views.Content
{
    public class TimelineView : UserControl
    {
        public TimelineView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}