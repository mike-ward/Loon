using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemView : UserControl, ICancellationTokeSourceProvider
    {
        public const string TweetItemImageName        = nameof(TweetItemImage);
        public const string TweetItemProfileImageName = nameof(TweetItemProfileImage);
        public const string TweetItemQuotedName       = nameof(TweetItemQuoted);
        public const string TweetItemRelatedName      = nameof(TweetItemRelated);

        public CancellationTokenSource CancellationTokenSource { get; set; } = new();

        public TweetItemView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContextChanged += OnDataContextChanged;
        }

        private async void OnDataContextChanged(object? sender, EventArgs e)
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource = new CancellationTokenSource();
            var token = CancellationTokenSource.Token;
            
            if (token.IsCancellationRequested is false &&
                DataContext is TwitterStatus status && 
                status.OriginatingStatus.RelatedLinkInfo is null)
            {
                status.OriginatingStatus.RelatedLinkInfo = await RelatedLinkInfo
                    .GetRelatedLinkInfoAsync(status.OriginatingStatus, token);
            }
        }
    }
}