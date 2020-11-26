using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using TweetX.Services;
using Twitter.Models;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemProfileImage : UserControl
    {
        private static readonly Func<string, ValueTask<IImage>> getMemoizedImage
            = MemoizeService.Memoize<string, ValueTask<IImage>>(uri => GetImage(uri), capacity: 50);

        public TweetItemProfileImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void UpdateImage(object? sender, EventArgs e)
        {
            if (DataContext is TwitterStatus status && sender is Image image)
            {
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        var uri = status.User.ProfileImageUrl;
                        if (uri is not null)
                        {
                            var bitmap = await getMemoizedImage(uri).ConfigureAwait(false);
                            await Dispatcher.UIThread.InvokeAsync(() => image.Source = bitmap).ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        // eat it
                    }
                });
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