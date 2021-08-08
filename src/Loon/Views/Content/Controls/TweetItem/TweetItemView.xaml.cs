using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemView : UserControl
    {
        public const string TweetItemImageName = nameof(TweetItemImage);
        public const string TweetItemProfileImageName = nameof(TweetItemProfileImage);
        public const string TweetItemQuotedName = nameof(TweetItemQuoted);
        public const string TweetItemRelatedName = nameof(TweetItemRelated);

        public TweetItemView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}