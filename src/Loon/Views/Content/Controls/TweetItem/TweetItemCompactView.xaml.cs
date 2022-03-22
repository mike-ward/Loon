using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    public sealed class TweetItemCompactView : UserControl
    {
        public TweetItemCompactView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}