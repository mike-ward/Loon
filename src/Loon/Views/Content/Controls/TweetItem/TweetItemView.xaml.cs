using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public sealed class TweetItemView : UserControl, ICancellationTokeSourceProvider
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

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void OnDataContextChanged(object? sender, EventArgs e)
        {
            try
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource.Dispose();
                CancellationTokenSource = new CancellationTokenSource();

                if (DataContext is not TwitterStatus status) return;

                try
                {
                    await Task.WhenAll(
                        GetRelatedLinkInfoAsync(status),
                        PreloadImagesAsync(status.OriginatingStatus.ExtendedEntities?.Media),
                        PreloadImagesAsync(status.OriginatingStatus.QuotedStatus?.ExtendedEntities?.Media));
                }
                finally
                {
                    IsVisible = true;
                    Opacity   = 1.0;
                    Padding   = new Thickness();
                }
            }
            catch (TaskCanceledException)
            {
                // expected, nothing to do
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private async Task GetRelatedLinkInfoAsync(TwitterStatus status)
        {
            var relatedLinkInfo =
                status.OriginatingStatus.RelatedLinkInfo ??
                await RelatedLinkInfo.GetRelatedLinkInfoAsync(status.OriginatingStatus, CancellationTokenSource.Token);

            await PreloadImagesAsync(relatedLinkInfo?.ImageTwitterStatus.ExtendedEntities?.Media);
            status.OriginatingStatus.RelatedLinkInfo = relatedLinkInfo;
        }

        private Task PreloadImagesAsync(Media[]? media)
        {
            if (media is null) return Task.CompletedTask;

            // Cut down on janking by preloading stuff
            return Task.WhenAll(media.Select(
                async m => await ImageService.GetImageAsync(m.MediaUrl, CancellationTokenSource.Token)));
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if ((e.KeyModifiers & KeyModifiers.Alt) != 0 && DataContext is TwitterStatus status)
            {
                e.Handled = true;
                var json = JsonSerializer.Serialize(status, new JsonSerializerOptions { WriteIndented = true });
                App.Commands.CopyToClipboard.Execute(json);
            }
        }
    }
}