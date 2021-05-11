using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemView : UserControl
    {
        public static readonly string TweetItemImageName        = nameof(TweetItemImage);
        public static readonly string TweetItemProfileImageName = nameof(TweetItemProfileImage);
        public static readonly string TweetItemQuotedName       = nameof(TweetItemQuoted);
        public static readonly string TweetItemRelatedName      = nameof(TweetItemRelated);

        public TweetItemView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}