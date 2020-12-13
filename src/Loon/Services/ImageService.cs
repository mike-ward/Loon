using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Loon.Extensions;
using Loon.Models;
using Loon.Views.Content.ImageViewer;
using Twitter.Models;

namespace Loon.Services
{
    public static class ImageService
    {
        public static async ValueTask<IImage?> GetImageAsync(string uri, Func<bool> Clearing)
        {
            try
            {
                return await TryGetImageAsync(uri, Clearing).ConfigureAwait(true);
            }
            catch
            {
                try
                {
                    return await TryGetImageAsync(uri, Clearing).ConfigureAwait(true);
                }
                catch
                {
                    return await TryGetImageAsync(uri, Clearing).ConfigureAwait(true);
                }
            }
        }

        public static async ValueTask<IImage?> TryGetImageAsync(string uri, Func<bool> Clearing)
        {
            if (Clearing()) return null;
            var wc = WebRequest.Create(uri);
            wc.Timeout = Constants.WebRequestTimeout;
            using var response = await wc.GetResponseAsync().ConfigureAwait(false);

            if (Clearing()) return null;
            using var stream = response.GetResponseStream();
            using var ms = new MemoryStream(); // Bitmap constructor needs a seekable stream
            await stream.CopyToAsync(ms).ConfigureAwait(false);

            if (Clearing()) return null;
            ms.Position = 0;
            return new Bitmap(ms);
        }

        private static ImageViewerWindow? imageViewer;

        public static ImageViewerWindow GetImageViewer()
        {
            if (imageViewer is null || imageViewer.IsClosed)
            {
                imageViewer = new ImageViewerWindow();
            }

            imageViewer.Hide();
            return imageViewer;
        }

        public static void CopyImageToClipboard(IImage? Source)
        {
            if (Source is not null)
            {
                var bitmap = (IBitmap)Source;
                var rtb = new RenderTargetBitmap(bitmap.PixelSize, new Vector(96, 96));
                using var ctxi = rtb.CreateDrawingContext(null);
                using var ctx = new DrawingContext(ctxi, false);
                var rect = new Rect(0, 0, bitmap.PixelSize.Width, bitmap.PixelSize.Height);

                Source.Draw(ctx, rect, rect, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.Default);
                var dataobj = new DataObject();

                dataobj.Set("DeviceIndependentBitmap", rtb);
                App.Current.Clipboard.SetDataObjectAsync(dataobj);
            }
        }

        private static Process? process;

        public static void OpenInViewer(Image image)
        {
            var viewer = GetImageViewer(); // call here to close now
            var videoUrl = VideoUrl((image.DataContext) as Media);

            if (videoUrl.IsPopulated())
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

                pi.Arguments = $"--ontop --no-border --window-scale=1.25 {videoUrl}";
                pi.CreateNoWindow = false;

                process?.Kill();
                process?.Close();
                process = null;

                process = Process.Start(pi);
            }
            else if (image.Source is not null)
            {
                process?.Kill();
                process?.Close();
                process = null;

                viewer.Source = image.Source;
                viewer.Show(App.MainWindow);
            }
        }

        public static bool IsMp4(string? url)
        {
            return url.IsPopulated() && url!.Contains(".mp4", StringComparison.OrdinalIgnoreCase);
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
    }
}