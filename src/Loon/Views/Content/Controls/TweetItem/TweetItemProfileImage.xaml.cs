using System;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;
using Loon.Services;
using Loon.ViewModels;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemProfileImage : UserControl
    {
        public bool Clearing { get; set; }

        private const int profileSize = 73; // Twitter's bigger profile image size is 48x48

        private static readonly Bitmap EmptyBitmap
            = new WriteableBitmap(new PixelSize(profileSize, profileSize), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);

        public TweetItemProfileImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void UpdateImage(object? sender, EventArgs e)
        {
            try
            {
                Clearing = false;

                if (sender is Image image)
                {
                    image.Source = EmptyBitmap;

                    await Task.Delay(30).ConfigureAwait(true);
                    if (Clearing)
                    {
                        return;
                    }

                    if (DataContext is TwitterStatus status)
                    {
                        var uri = status.User.ProfileImageUrlBigger;
                        if (uri is not null && uri.Length > 0 && !Clearing)
                        {
                            image.Source = await ImageService.GetImageAsync(uri, () => Clearing).ConfigureAwait(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        public void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed &&
                DataContext is TwitterStatus status &&
                status.User.ScreenName is string screenName &&
                this.FindAncestorOfType<Window>() is Window window &&
                window.DataContext is MainWindowViewModel vm)
            {
                e.Handled = true;
                vm.SetUserProfile(screenName);
            }
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