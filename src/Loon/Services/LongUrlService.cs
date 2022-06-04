using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Twitter.Services;

namespace Loon.Services
{
    internal static class LongUrlService
    {
        private const           int                                  maxCacheSize = 100;
        private static readonly ConcurrentDictionary<string, string> UrlCache     = new(1, maxCacheSize + 1, StringComparer.Ordinal);

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
                using var response = await OAuthApiRequest.MyHttpClient.SendAsync(request, tokenSource.Token).ConfigureAwait(false);

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

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private static async void IsOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            try
            {
                var control = e.Sender as TextBlock;
                if (control?.Tag is string link && e.NewValue is bool and true)
                {
                    var tip = await TryGetLongUrlAsync(link);
                    control.SetValue(ToolTip.TipProperty, tip);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}