using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Loon.Services
{
    public static class ImageMemoryCacheService
    {
        // Poor man's memory cache for images.
        private static readonly ConcurrentDictionary<string, WeakReference<IImage>> imageMemoryCache = new(StringComparer.OrdinalIgnoreCase);

        public static IImage? FromCache(string uri)
        {
            return imageMemoryCache.TryGetValue(uri, out var weakReference) && weakReference.TryGetTarget(out var image)
                ? image
                : default;
        }

        public static void ToCache(string uri, IImage image)
        {
            imageMemoryCache.AddOrUpdate(uri, new WeakReference<IImage>(image), (_, v) => v);
        }

        public static ValueTask PruneCache()
        {
            foreach (var (key, value) in imageMemoryCache)
            {
                if (value.TryGetTarget(out _) is false)
                {
                    imageMemoryCache.TryRemove(key, out _);
                }
            }

            return default;
        }
    }
}