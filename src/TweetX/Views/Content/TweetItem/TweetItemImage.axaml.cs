using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using TweetX.Models;
using TweetX.Services;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemImage : UserControl
    {
        public bool Clearing { get; set; }

        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void LoadMediaAsync(object? sender, EventArgs e)
        {
            try
            {
                Clearing = false;

                if (sender is Image image)
                {
                    image.Source = null;
                    await Task.Delay(30).ConfigureAwait(true);
                    if (Clearing) return;

                    if (image.DataContext is string uri && uri.Length > 0)
                    {
                        try
                        {
                            image.Source = await GetImageAsync(uri).ConfigureAwait(true);
                        }
                        catch
                        {
                            try
                            {
                                image.Source = await GetImageAsync(uri).ConfigureAwait(true);
                            }
                            catch
                            {
                                image.Source = await GetImageAsync(uri).ConfigureAwait(true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private async ValueTask<IImage?> GetImageAsync(string uri)
        {
            if (Clearing) return null;
            var wc = WebRequest.Create(uri);
            wc.Timeout = Constants.WebRequestTimeout;
            using var response = await wc.GetResponseAsync().ConfigureAwait(false);

            if (Clearing) return null;
            using var stream = response.GetResponseStream();
            using var ms = new MemoryStream(); // Bitmap constructor needs a seekable stream
            await stream.CopyToAsync(ms).ConfigureAwait(false);

            if (Clearing) return null;
            ms.Position = 0;
            var height = (int)Constants.ImagePanelHeight;
            return Bitmap.DecodeToHeight(ms, height, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality);
        }
    }
}