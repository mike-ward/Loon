using System;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemProfileImage : UserControl
    {
        private const int profileSize = 73; // Twitter's bigger profile image size is 48x48
        private static readonly Bitmap EmptyBitmap = new WriteableBitmap(new PixelSize(profileSize, profileSize), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);

        public TweetItemProfileImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

#pragma warning disable RCS1213 // (used in XAML) Remove unused member declaration.
#pragma warning disable S1144 // (used in XAML) Unused private types or members should be removedremoved

        private async void UpdateImage(object? sender, EventArgs e)
        {
            if (sender is Image image)
            {
                try
                {
                    image.Source =
                        DataContext is TwitterStatus status &&
                        status.User.ProfileImageUrlBigger is string uri &&
                        uri.Length > 0
                            ? await ImageService
                                .GetImageAsync(uri)
                                .ConfigureAwait(true)
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
            if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed &&
                DataContext is TwitterStatus status)
            {
                e.Handled = true;
                App.Commands.SetUserProfileContext.Execute(status.User);
            }
            // Useful for debugging twitter oddities
            //
            else if (e.GetCurrentPoint(null).Properties.IsRightButtonPressed &&
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