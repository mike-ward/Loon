using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemImage : UserControl
    {
        private CancellationToken cancellationToken = CancellationToken.None;

        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            var cancellationTokeSourceProvider = this.FindLogicalAncestorOfType<ICancellationTokeSourceProvider>();
            cancellationToken = cancellationTokeSourceProvider?.CancellationTokenSource.Token ?? CancellationToken.None;
            base.OnDataContextChanged(e);
        }

        private async void LoadMediaAsync(object? sender, EventArgs e)
        {
            var token = cancellationToken; // make a copy
            
            if (sender is Image { DataContext: Media media } image)
            {
                try
                {
                    image.Source = null;
                    if (token.IsCancellationRequested) return;

                    var imageSource = await ImageService
                        .GetImageAsync(media.MediaUrl, token)
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