using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemImage : UserControl
    {
        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void LoadMediaAsync(object? sender, EventArgs e)
        {
            if (sender is Image { DataContext: Media media } image)
            {
                try
                {
                    image.Source = null;
                    var cancellationTokeSourceProvider = this.FindAncestorOfType<ICancellationTokeSourceProvider>();
                    var cancellationToken              = cancellationTokeSourceProvider?.CancellationTokenSource.Token ?? CancellationToken.None;
                    if (cancellationToken.IsCancellationRequested) return;

                    var imageSource = await ImageService
                        .GetImageAsync(media.MediaUrl, cancellationToken)
                        .ConfigureAwait(true);

                    image.Source ??= imageSource;
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }
        }

        private void OpenInViewer(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed &&
                sender is Grid grid &&
                grid.Children[0] is Image image)
            {
                ImageService.OpenInViewer(image);
            }
        }
    }
}