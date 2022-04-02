using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Loon.Extensions;
using Loon.Views.Content.Controls;
using Twitter.Models;
using Twitter.Services;

namespace Loon.Services
{
    internal static class ImageService
    {
        public static async ValueTask<IImage?> GetImageAsync(string uri, CancellationToken cancellationToken)
        {
            var retries = 3;

            do
            {
                try
                {
                    return ImageMemoryCacheService.FromCache(uri)
                        ?? await ImageFileCacheService.FromCacheAsync(uri, cancellationToken).ConfigureAwait(false)
                        ?? await FromHttpAsync(uri, cancellationToken).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }

                TraceService.Message($"retry image: {uri}");
                await Task.Delay(500, cancellationToken).ConfigureAwait(false);
            } 
            while (retries-- > 0);

            return default;
        }

        private static async ValueTask<IImage?> FromHttpAsync(string uri, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return default;
            var response = await OAuthApiRequest.MyHttpClient.GetStreamAsync(uri, cancellationToken).ConfigureAwait(false);

            await using var ms = new MemoryStream(); // Bitmap constructor needs a seekable stream
            await response.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) return default;

            ms.Position = 0;
            var bitmap = new Bitmap(ms);
            ImageMemoryCacheService.ToCache(uri, bitmap);
            await ImageFileCacheService.ToCacheAsync(uri, bitmap, cancellationToken);
            return bitmap;
        }

        // --------------------------------------------------------------------

        private static ImageViewer? imageViewer;
        private static Process?     videoPlayerProcess;
        private static string?      previousVideoUrl;

        public static void OpenInViewer(Image image)
        {
            KillImageViewer();
            async void ActionOpenViewer() => await OpenInViewerTask(image);
            Dispatcher.UIThread.Post(ActionOpenViewer);
        }

        public static void KillImageViewer()
        {
            videoPlayerProcess?.Kill();
            videoPlayerProcess?.Close();
            videoPlayerProcess = null;
            imageViewer?.Close();
        }

        public static string VideoUrl(Media? media)
        {
            return media
              ?.VideoInfo
              ?.Variants
              ?.Select(variant => variant.Url)
               .First() ?? string.Empty;
        }

        private static async Task OpenInViewerTask(Image image)
        {
            var videoUrl = VideoUrl(image.DataContext as Media);

            if (videoUrl.IsNotNullOrWhiteSpace())
            {
                if (videoUrl.IsEqualTo(previousVideoUrl))
                {
                    previousVideoUrl = string.Empty;
                }
                else
                {
                    videoPlayerProcess = await PlayVideo(videoUrl);
                }
            }
            else if (image.Source is not null)
            {
                previousVideoUrl = string.Empty;
                imageViewer      = new ImageViewer(image.Source);
                imageViewer.Show(App.MainWindow);
            }
        }

        private static async Task<Process?> PlayVideo(string videoUrl)
        {
            var pi = new ProcessStartInfo
            {
                FileName       = "vlc",
                Arguments      = $"--one-instance --quiet --no-osd --no-skins2-taskbar {videoUrl}",
                CreateNoWindow = true
            };
            try
            {
                var process = Process.Start(pi);
                previousVideoUrl = videoUrl;
                return process;
            }
            catch (Win32Exception)
            {
                await MessageBox.Show(App.GetString("install-vlc"), MessageBox.MessageBoxButtons.Ok);
            }
            catch (InvalidOperationException)
            {
                await MessageBox.Show(App.GetString("install-vlc"), MessageBox.MessageBoxButtons.Ok);
            }

            return null;
        }

        // --------------------------------------------------------------------

        public static void CopyImageToClipboard(IImage? Source)
        {
            if (Source is not null && Application.Current?.Clipboard is not null)
            {
                // This space for rent
                var unused = Application.Current.Clipboard.SetTextAsync("bitmap copying not implemented");
            }
        }
    }
}