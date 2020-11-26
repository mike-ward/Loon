using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemImage : UserControl
    {
        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void LoadMedia(object? sender, EventArgs e)
        {
            try
            {
                if (sender is Image image && image.DataContext is string uri)
                {
                    image.Source = await GetImage(uri).ConfigureAwait(true);
                }
            }
            catch
            {
                // eat it for now
            }
        }

        private static async ValueTask<IImage> GetImage(string uri)
        {
            var response = await App.Http.GetAsync(uri, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return new Bitmap(stream);
        }
    }
}