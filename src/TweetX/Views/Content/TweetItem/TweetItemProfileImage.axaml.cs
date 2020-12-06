using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using TweetX.Models;
using Twitter.Models;

namespace TweetX.Views.Content.TweetItem
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
                    if (Clearing) return;

                    if (DataContext is TwitterStatus status)
                    {
                        var uri = status.User.ProfileImageUrlBigger;
                        if (uri is not null && uri.Length > 0 && !Clearing)
                        {
                            try
                            {
                                image.Source = await GetImage(uri).ConfigureAwait(true);
                            }
                            catch
                            {
                                try
                                {
                                    image.Source = await GetImage(uri).ConfigureAwait(true);
                                }
                                catch
                                {
                                    image.Source = await GetImage(uri).ConfigureAwait(true);
                                }
                            }
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
            var wc = WebRequest.Create(uri);
            wc.Timeout = Constants.WebRequestTimeout;
            using var response = await wc.GetResponseAsync().ConfigureAwait(false);

            if (Clearing) return null;
            using var stream = response.GetResponseStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms).ConfigureAwait(false);

            if (Clearing) return null;
            ms.Position = 0;
            return Bitmap.DecodeToHeight(ms, profileSize);
        }
    }
}