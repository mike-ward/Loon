using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Twitter.Models;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemProfileImage : UserControl
    {
        public TweetItemProfileImage()
        {
            InitializeComponent();
            DataContextChanged += UpdateImage;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void UpdateImage(object? sender, EventArgs e)
        {
            base.OnInitialized();

            try
            {
                var image = this.FindControl<Image>("Image");
                image.Source = null;

                if (DataContext is TwitterStatus status)
                {
                    var uri = status.User.ProfileImageUrl;
                    if (uri is not null)
                    {
                        image.Source = await GetImage(uri).ConfigureAwait(true);
                    }
                }
            }
            catch
            {
                // eat it for now
            }
        }

        private static readonly ConcurrentDictionary<string, IImage> imageCache = new(StringComparer.Ordinal);

        private static async ValueTask<IImage> GetImage(string uri)
        {
            if (!imageCache.TryGetValue(uri, out var bitmap))
            {
                var response = await App.Http.GetAsync(uri, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                bitmap = new Bitmap(stream);
                imageCache.TryAdd(uri, bitmap);
            }

            return bitmap;
        }
    }
}