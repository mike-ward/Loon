using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Loon.Extensions;
using Loon.Views.Content.Controls;
using Twitter.Models;
using Twitter.Services;

namespace Loon.Services
{
    internal static class ImageService
    {
        private static readonly string TempPath = Path.GetTempPath();

        public static async ValueTask<IImage?> GetImageAsync(string uri, CancellationToken cancellationToken)
        {
            const int retries = 3;

            for (var retry = 0; retry < retries; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        if (cancellationToken.IsCancellationRequested) return default;
                        TraceService.Message($"retry image: {retry}");
                        await Task.Delay(500, cancellationToken).ConfigureAwait(false);
                    }

                    if (cancellationToken.IsCancellationRequested) return default;

                    return await FromCacheAsync(uri, cancellationToken).ConfigureAwait(false)
                        ?? await ImageGetAsync(uri, cancellationToken).ConfigureAwait(false);
                }
                catch (FormatException ex)
                {
                    TraceService.Message(ex.Message);
                    break;
                }
                catch (ArgumentException ex)
                {
                    TraceService.Message(ex.Message);
                    break;
                }
                catch (FileNotFoundException ex)
                {
                    TraceService.Message(ex.Message);
                    break;
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }

            return default;
        }

        private static async ValueTask<IImage?> FromCacheAsync(string uri, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested) return default;
                var path = CachePathFromUrl(uri);

                return File.Exists(path)
                    ? await GetBitmapAsync(path, cancellationToken).ConfigureAwait(false)
                    : default;
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
                return default;
            }
        }

        private static string CachePathFromUrl(string uri)
        {
            var hash = MD5.HashData(Encoding.UTF8.GetBytes(uri));
            return Path.Combine(TempPath, "loon-" + Convert.ToHexString(hash));
        }

        private static async ValueTask<IImage?> GetBitmapAsync(string url, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return default;
            var bytes = await File.ReadAllBytesAsync(url, cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) return default;
            await using var stream = new MemoryStream(bytes);
            return new Bitmap(stream);
        }

        private static async ValueTask<IImage?> ImageGetAsync(string uri, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return default;
            var response = await OAuthApiRequest.MyHttpClient.GetStreamAsync(uri, cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) return default;

            await using var ms = new MemoryStream(); // Bitmap constructor needs a seekable stream
            await response.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) return default;

            ms.Position = 0;
            var bitmap = new Bitmap(ms);
            await Task.Factory.StartNew(() => ToCache(uri, bitmap, cancellationToken), cancellationToken, TaskCreationOptions.None, TaskScheduler.Default).ConfigureAwait(false);

            return cancellationToken.IsCancellationRequested
                ? default
                : bitmap;
        }

        private static void ToCache(string uri, Bitmap image, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested) return;
                using var ms = new MemoryStream();
                image.Save(ms);
                File.WriteAllBytes(CachePathFromUrl(uri), ms.ToArray());
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private static ImageViewer? imageViewer;

        private static ImageViewer GetImageViewer()
        {
            imageViewer?.Close();
            imageViewer = new ImageViewer();
            imageViewer.Hide();

            process?.Kill();
            process?.Close();
            process = null;

            return imageViewer;
        }

        public static void CopyImageToClipboard(IImage? Source)
        {
            if (Source is not null && Application.Current?.Clipboard is not null)
            {
                // This space for rent
                var unused = Application.Current.Clipboard.SetTextAsync("bitmap copying not implemented");
            }
        }

        private static Process? process;

        public static async Task OpenInViewer(Image image)
        {
            process?.Kill();
            process?.Close();
            process = null;

            // Click on an image already being viewed closes it.
            if (imageViewer?.Source == image.Source)
            {
                imageViewer.Close();
                imageViewer.Source = null;
                imageViewer.Hide();
                return;
            }

            var viewer   = GetImageViewer(); // call here to close now
            var videoUrl = VideoUrl((image.DataContext) as Media);

            if (videoUrl.IsNotNullOrWhiteSpace())
            {
                var pi = new ProcessStartInfo
                {
                    FileName        = "vlc",
                    Arguments       = $"--loop --quiet --no-osd {videoUrl}",
                    UseShellExecute = true,
                    CreateNoWindow  = true
                };
                process = Process.Start(pi);
                if (process is null) await MessageBox.Show(App.GetString("install-vlc"), MessageBox.MessageBoxButtons.Ok);
            }
            else if (image.Source is not null)
            {
                viewer.Source = image.Source;
                viewer.Show(App.MainWindow);
            }
        }

        public static string VideoUrl(Media? media)
        {
            if (media?.VideoInfo?.Variants is null)
            {
                return string.Empty;
            }

            return media.VideoInfo.Variants
                      .Select(variant => variant.Url)
                      .FirstOrDefault()
                ?? string.Empty;
        }

        public static void ClearImageCache()
        {
            foreach (var file in Directory.GetFiles(TempPath, "loon-*"))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // don't care
                }
            }
        }
    }
}