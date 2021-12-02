using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace Loon.Services
{
    internal static class LongUrlService
    {
        private const           int                                  maxCacheSize = 100;
        private static readonly ConcurrentDictionary<string, string> UrlCache     = new(concurrencyLevel: 1, capacity: maxCacheSize + 1, comparer: StringComparer.Ordinal);

        static LongUrlService()
        {
            ToolTip.IsOpenProperty.Changed.Subscribe(IsOpenChanged);
        }

        private static async ValueTask<string> TryGetLongUrlAsync(string link)
        {
            try
            {
                if (UrlCache.TryGetValue(link, out var longUrl))
                {
                    return longUrl;
                }

                const int FiveSeconds = 5000;
                using var tokenSource = new CancellationTokenSource(FiveSeconds);

                var       request  = new HttpRequestMessage { Method = HttpMethod.Head, RequestUri = new Uri(link) };
                using var response = await Twitter.Services.OAuthApiRequest.MyHttpClient.SendAsync(request, tokenSource.Token).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var uri = response.RequestMessage?.RequestUri?.AbsoluteUri;

                    if (!string.IsNullOrWhiteSpace(uri))
                    {
                        if (UrlCache.Count > maxCacheSize)
                        {
                            UrlCache.Clear();
                        }

                        UrlCache.TryAdd(link, uri);
                        return uri;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }

            return link;
        }

        private static async void IsOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var control = e.Sender as TextBlock;
            if (control?.Tag is string link && e.NewValue is true)
            {
                var tip = await TryGetLongUrlAsync(link);
                control.SetValue(ToolTip.TipProperty, tip);
            }
        }
    }
}