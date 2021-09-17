using System.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemView : UserControl, ICancellationTokeSourceProvider
    {
        public const string TweetItemImageName = nameof(TweetItemImage);
        public const string TweetItemProfileImageName = nameof(TweetItemProfileImage);
        public const string TweetItemQuotedName = nameof(TweetItemQuoted);
        public const string TweetItemRelatedName = nameof(TweetItemRelated);

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        public TweetItemView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}