using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
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
            } while (retries-- > 0);

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