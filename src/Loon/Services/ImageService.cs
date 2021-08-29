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

        public static async ValueTask<IImage?> GetImageAsync(string uri)
        {
            const int retries = 3;

            for (var retry = 0; retry < retries; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        TraceService.Message($"retry image: {retry}");
                        await Task.Delay(500).ConfigureAwait(false);
                    }

                    return await FromCacheAsync(uri).ConfigureAwait(false)
                        ?? await ImageGetAsync(uri).ConfigureAwait(false);
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
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }

            return default;
        }

        private static async ValueTask<IImage?> FromCacheAsync(string uri)
        {
            try
            {
                var path = GetPath(uri);

                return File.Exists(path)
                    ? await GetBitmapAsync(path).ConfigureAwait(false)
                    : default;
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
                return default;
            }
        }

        private static string GetPath(string uri)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(uri));

            return Path.Combine(
                TempPath,
                "loon-" + Convert.ToHexString(hash));
        }

        private static async ValueTask<IImage?> GetBitmapAsync(string url)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
            var             bytes  = await File.ReadAllBytesAsync(url, cts.Token).ConfigureAwait(false);
            await using var stream = new MemoryStream(bytes);
            return new Bitmap(stream);
        }

        private static async ValueTask<IImage?> ImageGetAsync(string uri)
        {
            var response = await OAuthApiRequest.MyHttpClient.GetStreamAsync(uri).ConfigureAwait(false);

            await using var ms = new MemoryStream(); // Bitmap constructor needs a seekable stream
            await response.CopyToAsync(ms).ConfigureAwait(false);

            ms.Position = 0;
            var bitmap = new Bitmap(ms);
            await ToCacheAsync(uri, bitmap).ConfigureAwait(false);
            return bitmap;
        }

        private static async ValueTask ToCacheAsync(string uri, Bitmap image)
        {
            try
            {
                await using var ms = new MemoryStream();
                image.Save(ms);
                var cts = new CancellationTokenSource();
                cts.CancelAfter(1000);
                await File.WriteAllBytesAsync(GetPath(uri), ms.ToArray(), cts.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private static ImageViewer? imageViewer;

        private static ImageViewer GetImageViewer()
        {
            if (imageViewer is null || imageViewer.IsClosed)
            {
                imageViewer = new ImageViewer();
            }

            imageViewer.Hide();

            process?.Kill();
            process?.Close();
            process = null;

            return imageViewer;
        }

        public static void CopyImageToClipboard(IImage? Source)
        {
            if (Source is not null)
            {
                // This space for rent
                Application.Current.Clipboard.SetTextAsync("bitmap copying not implemented");
            }
        }

        private static Process? process;

        public static void OpenInViewer(Image image)
        {
            var viewer   = GetImageViewer(); // call here to close now
            var videoUrl = VideoUrl((image.DataContext) as Media);

            if (videoUrl.IsNotNullOrWhiteSpace())
            {
                var pi = new ProcessStartInfo();

                if (OperatingSystem.IsWindows())
                {
                    pi.FileName = Path.Combine(
                        AppContext.BaseDirectory,
                        "Assets/Windows/mpv.exe");

                    pi.Arguments = $"--ontop --no-border --autofit-smaller=640x480 --keep-open --script-opts=osc-scalewindowed=3 {videoUrl}";
                }
                else if (OperatingSystem.IsLinux())
                {
                    pi.UseShellExecute = true;
                    pi.FileName        = videoUrl;
                }
                else
                {
                    return;
                }

                pi.CreateNoWindow = false;
                process           = Process.Start(pi);
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