using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Loon.Extensions;
using Loon.Models;
using Loon.Views.Content.Controls;
using Twitter.Models;

namespace Loon.Services
{
    internal static class ImageService
    {
        private static readonly string TempPath = Path.GetTempPath();

        public static async ValueTask<IImage?> GetImageAsync(string uri)
        {
            int delay = 200;
            const int retries = 3;
            const int backoffMultiplier = 3;

            for (var retry = 0; retry < retries; retry++)
            {
                try
                {
                    return FromCache(uri) is IImage image
                        ? image
                        : await TryGetImageAsync(uri).ConfigureAwait(false);
                }
                catch (FormatException ex)
                {
                    TraceService.Message(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    TraceService.Message(ex.Message);
                }
                catch (FileNotFoundException ex)
                {
                    TraceService.Message(ex.Message);
                }
                catch
                {
                    delay *= backoffMultiplier;
                }
            }

            return default;
        }

        public static async ValueTask<IImage?> TryGetImageAsync(string uri)
        {
            var wc = WebRequest.Create(uri);
            wc.Timeout = Constants.WebRequestTimeout;
            using var response = await wc.GetResponseAsync().ConfigureAwait(false);

            using var stream = response.GetResponseStream();
            using var ms = new MemoryStream(); // Bitmap constructor needs a seekable stream
            await stream.CopyToAsync(ms).ConfigureAwait(false);

            ms.Position = 0;
            var bitmap = new Bitmap(ms);
            ToCache(uri, bitmap);
            return bitmap;
        }

        private static ImageViewer? imageViewer;

        public static ImageViewer GetImageViewer()
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
                App.Current.Clipboard.SetTextAsync("bitmap copying not implemented");
            }
        }

        private static Process? process;

        public static void OpenInViewer(Image image)
        {
            var viewer = GetImageViewer(); // call here to close now
            var videoUrl = VideoUrl((image.DataContext) as Media);

            if (videoUrl.IsNotNullOrWhiteSpace())
            {
                var pi = new ProcessStartInfo();

                if (OperatingSystem.IsWindows())
                {
                    pi.FileName = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                        "Assets/Windows/mpv.exe");
                }
                else
                {
                    return;
                }

                pi.Arguments = $"--ontop --no-border --autofit-smaller=640x480 --keep-open --script-opts=osc-scalewindowed=3 {videoUrl}";
                pi.CreateNoWindow = false;
                process = Process.Start(pi);
            }
            else if (image.Source is not null)
            {
                viewer.Source = image.Source;
                viewer.Show(App.MainWindow);
            }
        }

        public static string VideoUrl(Media? media)
        {
            if (media is null || media.VideoInfo is null || media.VideoInfo.Variants is null)
            {
                return string.Empty;
            }

            return media.VideoInfo.Variants
                .Select(variant => variant.Url)
                .FirstOrDefault()
                ?? string.Empty;
        }

        private static void ToCache(string uri, Bitmap image)
        {
            var path = GetPath(uri);
            try
            {
                image.Save(path);
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private static IImage? FromCache(string uri)
        {
            try
            {
                var path = GetPath(uri);

                return File.Exists(path)
                    ? new Bitmap(path)
                    : null;
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
                return null;
            }
        }

        private static string GetPath(string uri)
        {
            return Path.Combine(
                TempPath,
                "loon-" + Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(uri))));
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