using System;
using System.Text.Json;
using System.Threading;
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
    internal class TweetItemProfileImage : UserControl
    {
        private const           int               profileSize       = 73; // Twitter's bigger profile image size is 48x48
        private                 CancellationToken cancellationToken = CancellationToken.None;
        private static readonly Bitmap            EmptyBitmap       = new WriteableBitmap(new PixelSize(profileSize, profileSize), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);

        public TweetItemProfileImage()
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

        private async void UpdateImage(object? sender, EventArgs _)
        {
            var token = cancellationToken;
            
            if (sender is Image image)
            {
                try
                {
                    image.Source = null;
                    if (token.IsCancellationRequested) return;

                    var imageSource =
                        DataContext is TwitterStatus status &&
                        status.User.ProfileImageUrlBigger is { Length: > 0 } uri
                            ? await ImageService
                                .GetImageAsync(uri, token)
                                .ConfigureAwait(true)
                            : EmptyBitmap;

                    image.Source = image.Source is null
                        ? imageSource
                        : EmptyBitmap;
                }
                catch (Exception ex)
                {
                    image.Source = EmptyBitmap;
                    TraceService.Message(ex.Message);
                }
            }
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(relativeTo: null).Properties.IsLeftButtonPressed &&
                DataContext is TwitterStatus status)
            {
                e.Handled = true;
                App.Commands.SetUserProfileContext.Execute(status.User);
            }
            // Useful for debugging twitter oddities
            //
            else if (e.GetCurrentPoint(relativeTo: null).Properties.IsRightButtonPressed &&
                e.KeyModifiers.HasFlag(KeyModifiers.Control) &&
                DataContext is TwitterStatus status1)
            {
                e.Handled = true;
                var json = JsonSerializer.Serialize(status1, new JsonSerializerOptions { WriteIndented = true });
                _ = Application.Current.Clipboard.SetTextAsync(json);
            }
        }
    }
}