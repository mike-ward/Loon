using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines.TweetItem
{
    public class TweetItemRetweetedBy : UserControl
    {
        public TweetItemRetweetedBy()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}