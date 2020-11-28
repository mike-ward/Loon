using System;
using System.Collections.Generic;
using BitFaster.Caching.Lru;

namespace TweetX.Services
{
    public static class MemoizeService
    {
        public static Func<TKey, TResult> Memoize<TKey, TResult>(this Func<TKey, TResult> func, int capacity) where TKey : notnull
        {
            var comparer = func is Func<string, TResult>
                ? StringComparer.Ordinal as IEqualityComparer<TKey>
                : EqualityComparer<TKey>.Default;

            var cache = new ConcurrentLru<TKey, TResult>(Environment.ProcessorCount, capacity, comparer);
            return key => cache.GetOrAdd(key, func);
        }
    }
}