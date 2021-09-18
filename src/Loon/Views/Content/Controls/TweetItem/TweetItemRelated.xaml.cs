using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemRelated : UserControl
    {
        public TweetItemRelated()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OpenUrl(string link)
        {
            Services.OpenUrlService.Open(link);
        }
    }
}