using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Loon.Services
{
    public static class ImageFileCacheService
    {
        public static async ValueTask<IImage?> FromCacheAsync(string uri, CancellationToken cancellationToken)
        {
            var path = CachePathFromUrl(uri);
            if (File.Exists(path) is false) return default;
            try
            {
                var bytes = await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested) return default;
                await using var stream = new MemoryStream(bytes);
                var             image  = new Bitmap(stream);
                ImageMemoryCacheService.ToCache(uri, image); // for faster access next time
                return image;
            }
            catch
            {
                return default;
            }
        }

        public static async ValueTask ToCacheAsync(string uri, Bitmap image, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested) return;
                await using var ms = new MemoryStream();
                image.Save(ms);
                await File.WriteAllBytesAsync(CachePathFromUrl(uri), ms.ToArray(), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        public static void ClearCache()
        {
            foreach (var file in Directory.GetFiles(Path.GetTempPath(), "loon-*"))
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

        private static string CachePathFromUrl(string uri)
        {
            var hash = MD5.HashData(Encoding.UTF8.GetBytes(uri));
            return Path.Combine(Path.GetTempPath(), "loon-" + Convert.ToHexString(hash));
        }
    }
}