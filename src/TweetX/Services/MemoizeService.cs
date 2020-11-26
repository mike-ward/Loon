using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TweetX.Services
{
    public static class MemoizeService
    {
        public static Func<TKey, TResult> Memoize<TKey, TResult>(this Func<TKey, TResult> func) where TKey : notnull
        {
            var comparer = func is Func<string, TResult>
                ? StringComparer.Ordinal as IEqualityComparer<TKey>
                : EqualityComparer<TKey>.Default;

            var cache = new ConcurrentDictionary<TKey, Lazy<TResult>>(comparer);
            return key => cache.GetOrAdd(key, new Lazy<TResult>(() => func(key))).Value;
        }
    }
}