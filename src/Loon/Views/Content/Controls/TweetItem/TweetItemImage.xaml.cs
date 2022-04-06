using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public sealed class TweetItemImage : UserControl
    {
        private CancellationToken cancellationToken = CancellationToken.None;

        public TweetItemImage()
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
        private async void LoadMediaAsync(object? sender, EventArgs unused)
        {
            try
            {
                var token = cancellationToken; // make a copy
                if (token.IsCancellationRequested) return;

                if (sender is Image { DataContext: Media media } image)
                {
                    try
                    {
                        var imageSource = await ImageService.GetImageAsync(media.MediaUrl, token);
                        if (token.IsCancellationRequested) return;
                        image.Source ??= imageSource;
                    }
                    catch (TaskCanceledException)
                    {
                        // return
                    }
                    catch (Exception ex)
                    {
                        TraceService.Message(ex.Message);
                        image.Source = null;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private void OpenInViewer(object? sender, PointerPressedEventArgs e)
        {
            try
            {
                if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed &&
                    sender is Grid grid &&
                    grid.Children[0] is Image image)
                {
                    e.Handled = true;

                    if ((e.KeyModifiers & KeyModifiers.Control) != 0)
                    {
                        var mediaUrl = (image.DataContext as Media)?.MediaUrl;
                        App.Commands.AddToHiddenImages.Execute((mediaUrl, this.PointToScreen(new Point(5, 10))));
                    }
                    else
                    {
                        ImageViewerService.OpenInViewer(image);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}