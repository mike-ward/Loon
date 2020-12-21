using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines.TweetItem
{
    public class TweetItemRelated : UserControl
    {
        public TweetItemRelated()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenUrl(string link)
        {
            Services.OpenUrlService.Open(link);
        }
    }
}