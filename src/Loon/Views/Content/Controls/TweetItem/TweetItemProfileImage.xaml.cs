using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public sealed class TweetItemProfileImage : UserControl
    {
        private const           int               profileSize       = 73; // Twitter's bigger profile image size is 48x48
        private                 CancellationToken cancellationToken = CancellationToken.None;
        private static readonly Bitmap            EmptyBitmap       = new WriteableBitmap(new PixelSize(profileSize, profileSize), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);

        public TweetItemProfileImage()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            cancellationToken = this.FindLogicalAncestorOfType<ICancellationTokeSourceProvider>()?.CancellationTokenSource.Token ?? CancellationToken.None;
            base.OnDataContextChanged(e);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void UpdateImage(object? sender, EventArgs _)
        {
            try
            {
                var token = cancellationToken; // make a copy
                if (token.IsCancellationRequested) return;

                if (sender is Image { DataContext: TwitterStatus status } image)
                {
                    try
                    {
                        image.Source = null!;

                        var imageSource =
                            status.User.ProfileImageUrlBigger is { Length: > 0 } uri
                                ? await ImageService.GetImageAsync(uri, token)
                                : EmptyBitmap;

                        if (token.IsCancellationRequested) return;
                        image.Source = imageSource!;
                    }
                    catch (TaskCanceledException)
                    {
                        // return
                    }
                    catch (Exception ex)
                    {
                        TraceService.Message(ex.Message);
                        image.Source = EmptyBitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(null).Properties.IsLeftButtonPressed || DataContext is not TwitterStatus status) return;
            e.Handled = true;
            App.Commands.SetUserProfileContext.Execute(status.User);
        }
    }
}