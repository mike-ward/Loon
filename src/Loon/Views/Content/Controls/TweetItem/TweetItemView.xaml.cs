using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
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