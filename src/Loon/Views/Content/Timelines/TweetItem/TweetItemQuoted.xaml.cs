using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines.TweetItem
{
    public class TweetItemQuoted : UserControl
    {
        public TweetItemQuoted()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}