using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemView : UserControl, ICancellationTokeSourceProvider
    {
        public const string TweetItemImageName        = nameof(TweetItemImage);
        public const string TweetItemProfileImageName = nameof(TweetItemProfileImage);
        public const string TweetItemQuotedName       = nameof(TweetItemQuoted);
        public const string TweetItemRelatedName      = nameof(TweetItemRelated);

        public CancellationTokenSource CancellationTokenSource { get; private set; } = new();

        public TweetItemView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContextChanged += OnDataContextChanged;
        }

        private async void OnDataContextChanged(object? sender, EventArgs e)
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = new CancellationTokenSource();

            if (DataContext is TwitterStatus status)
            {
                try
                {
                    status.OriginatingStatus.RelatedLinkInfo ??= await RelatedLinkInfo
                        .GetRelatedLinkInfoAsync(status.OriginatingStatus, CancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // expected
                }
            }
        }
    }
}