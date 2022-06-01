using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter.Extensions;
using Twitter.Models;

namespace Twitter.Services
{
    public sealed class OAuthApiRequest
    {
        private const string     GET  = "GET";
        public const  string     POST = "POST";
        public static HttpClient MyHttpClient { get; } = new();

        private string? ConsumerKey       { get; }
        private string? ConsumerSecret    { get; }
        private string? AccessToken       { get; set; }
        private string? AccessTokenSecret { get; set; }

        public OAuthApiRequest(string? consumerKey, string? consumerSecret)
        {
            ConsumerKey    = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public void AuthenticationTokens(string? accessToken, string? accessTokenSecret)
        {
            AccessToken       = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public ValueTask GetAsync(string url, IEnumerable<(string, string)> parameters)
            => RequestAsync(url, parameters, GET);

        public ValueTask<T> GetAsync<T>(string url, IEnumerable<(string, string)> parameters) where T : class
            => RequestAsync<T>(url, parameters, GET);

        public ValueTask PostAsync(string url, IEnumerable<(string, string)> parameters)
            => RequestAsync(url, parameters, POST);

        public ValueTask<T> PostAsync<T>(string url, IEnumerable<(string, string)> parameters) where T : class
            => RequestAsync<T>(url, parameters, POST);

        private ValueTask RequestAsync(string url, IEnumerable<(string, string)> parameters, string method)
            => OAuthRequestAsync<object>(url, parameters, method).AsValueTask();

        private ValueTask<T> RequestAsync<T>(string url, IEnumerable<(string, string)> parameters, string method) where T : class
            => OAuthRequestAsync<T>(url, parameters, method);

        /// <summary>
        ///     Builds, signs and delivers an OAuth Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        private async ValueTask<T> OAuthRequestAsync<T>(string url, IEnumerable<(string, string)> parameters, string method) where T : class
        {
            var isPost           = string.Equals(method, POST, StringComparison.Ordinal);
            var nonce            = OAuth.Nonce();
            var timestamp        = OAuth.TimeStamp();
            var parray           = parameters.ToArray();
            var signature        = OAuth.Signature(method, url, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, parray);
            var authorizeHeader  = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);
            var parameterStrings = parray.Select(p => $"{OAuth.UrlEncode(p.Item1)}={OAuth.UrlEncode(p.Item2)}");

            using var request = new HttpRequestMessage();
            request.Headers.Add("Authorization", authorizeHeader);

            if (isPost)
            {
                request.Method                      = HttpMethod.Post;
                request.Content                     = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(string.Join("&", parameterStrings))));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            }
            else
            {
                request.Method =  HttpMethod.Get;
                url            += $"?{string.Join("&", parameterStrings)}";
            }

            request.RequestUri = new Uri(url);
            using var response = await MyHttpClient.SendAsync(request);
            var       stream   = await response.Content.ReadAsStreamAsync();
            return await DeserializeResult<T>(stream) ?? throw new InvalidOperationException("JsonSerializer.DeserializeAsync<T>(stream) returned null");
        }

        private static async ValueTask<T?> DeserializeResult<T>(Stream stream) where T : class
        {
            var typeT        = typeof(T);
            var typeUser     = typeof(User);
            var typeStatuses = typeof(IEnumerable<TwitterStatus>);

            return typeT switch
            {
                _ when typeT == typeUser     => await JsonSerializer.DeserializeAsync(stream, typeUser, TwitterStatusContext.Default).ConfigureAwait(false) as T,
                _ when typeT == typeStatuses => await JsonSerializer.DeserializeAsync(stream, typeStatuses, TwitterStatusContext.Default).ConfigureAwait(false) as T,
                _                            => await JsonSerializer.DeserializeAsync<T>(stream).ConfigureAwait(false)
            };
        }

        /// <summary>
        ///     Twitter requires media upload to be multipart form with specific parameters
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async ValueTask AppendMediaAsync(string mediaId, int segmentIndex, byte[] payload)
        {
            var          nonce           = OAuth.Nonce();
            var          timestamp       = OAuth.TimeStamp();
            const string uploadUrl       = TwitterApi.UploadMediaUrl;
            var          signature       = OAuth.Signature(POST, uploadUrl, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, null);
            var          authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);

            using var request = new HttpRequestMessage();
            request.Headers.Add("Authorization", authorizeHeader);
            request.Method = HttpMethod.Post;

            var boundary = $"{Guid.NewGuid():N}";
            request.Headers.Add("ContentType", "multipart/form-data; boundary=" + boundary);

            var stream = new MemoryStream();
            await TextParameterAsync(stream, boundary, "command", "APPEND").ConfigureAwait(false);
            await TextParameterAsync(stream, boundary, "media_id", mediaId).ConfigureAwait(false);
            await TextParameterAsync(stream, boundary, "segment_index", segmentIndex.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await BinaryParameterAsync(stream, boundary, "media", payload).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, $"--{boundary}--\r\n").ConfigureAwait(false);
            await stream.FlushAsync();

            request.Content = new StreamContent(stream);
            await MyHttpClient.SendAsync(request).ConfigureAwait(false);
        }

        private static async ValueTask TextParameterAsync(Stream stream, string boundary, string name, string payload)
        {
            var header = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n";
            await WriteTextToStreamAsync(stream, header).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, payload).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, "\r\n").ConfigureAwait(false);
        }

        private static async ValueTask BinaryParameterAsync(Stream stream, string boundary, string name, byte[] payload)
        {
            var header =
                $"--{boundary}\r\nContent-Type: application/octet-stream\r\n" +
                $"Content-Disposition: form-data; name=\"{name}\"\r\n\r\n";

            await WriteTextToStreamAsync(stream, header).ConfigureAwait(false);
            await stream.WriteAsync(payload.AsMemory(0, payload.Length)).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, "\r\n").ConfigureAwait(false);
        }

        private static ValueTask WriteTextToStreamAsync(Stream stream, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return stream.WriteAsync(buffer.AsMemory(0, buffer.Length));
        }
    }
}