using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using TweetX.Services;
using Twitter.Models;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemProfileImage : UserControl
    {
        public bool Clearing { get; set; }

        private const int profileSize = 48; // Twitter's normal profile image size is 48x48

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
                    if (Clearing) return;

                    if (DataContext is TwitterStatus status)
                    {
                        var uri = status.User.ProfileImageUrl;
                        if (uri is not null && uri.Length > 0 && !Clearing)
                        {
                            image.Source = await GetImage(uri).ConfigureAwait(true);
                        }
                    }
                }
            }
            catch
            {
                // eat it.
            }
        }

        private async ValueTask<IImage?> GetImage(string uri)
        {
            if (Clearing) return null;
            using var response = await HttpService.Http.GetAsync(uri).ConfigureAwait(false);

            if (Clearing) return null;
            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            if (Clearing) return null;
            return new Bitmap(stream);
        }
    }
}