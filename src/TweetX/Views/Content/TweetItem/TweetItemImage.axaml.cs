using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

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

        public void LoadMedia(object? sender, EventArgs e)
        {
            if (sender is Image image && image.DataContext is string uri)
            {
                try
                {
                    Task.Factory.StartNew(async () =>
                    {
                        var bitmap = await GetImage(uri).ConfigureAwait(false);
                        await Dispatcher.UIThread.InvokeAsync(() => image.Source = bitmap).ConfigureAwait(false);
                    });
                }
                catch
                {
                    // eat it
                }
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