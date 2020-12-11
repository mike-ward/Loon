using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines.TweetItem
{
    public class TweetItemAuthor : UserControl
    {
        public TweetItemAuthor()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenInTwitter(string par)
        {
            Services.OpenUrlService.Open(par);
        }
    }
}