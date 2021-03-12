﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Twitter.Services
{
    public sealed class OAuthApiRequest
    {
        public const  string     GET  = "GET";
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
        {
            return RequestAsync(url, parameters, GET);
        }

        public ValueTask<T> GetAsync<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return RequestAsync<T>(url, parameters, GET);
        }

        public ValueTask PostAsync(string url, IEnumerable<(string, string)> parameters)
        {
            return RequestAsync(url, parameters, POST);
        }

        public ValueTask<T> PostAsync<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return RequestAsync<T>(url, parameters, POST);
        }

        private async ValueTask RequestAsync(string url, IEnumerable<(string, string)> parameters, string method)
        {
            _ = await RequestAsync<object>(url, parameters, method).ConfigureAwait(false);
        }

        private ValueTask<T> RequestAsync<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            return OAuthRequestAsync<T>(url, parameters, method);
        }

        /// <summary>
        /// Builds, signs and delivers an OAuth Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private async ValueTask<T> OAuthRequestAsync<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            var post             = string.Equals(method, POST, StringComparison.Ordinal);
            var nonce            = OAuth.Nonce();
            var timestamp        = OAuth.TimeStamp();
            var parray           = parameters.ToArray();
            var signature        = OAuth.Signature(method, url, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, parray);
            var authorizeHeader  = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);
            var parameterStrings = parray.Select(p => $"{OAuth.UrlEncode(p.Item1)}={OAuth.UrlEncode(p.Item2)}");

            var request = new HttpRequestMessage();
            request.Headers.Add("Authorization", authorizeHeader);

            if (post)
            {
                request.Method  = HttpMethod.Post;
                request.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(string.Join("&", parameterStrings))));
            }
            else
            {
                request.Method =  HttpMethod.Get;
                url            += $"?{string.Join("&", parameterStrings)}";
            }

            request.RequestUri = new Uri(url);
            using var response = await MyHttpClient.SendAsync(request);
            var       result   = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync()).ConfigureAwait(false);
            return result ?? throw new InvalidOperationException("JsonSerializer.DeserializeAsync<T>(stream) return null");
        }

        /// <summary>
        /// Twitter requires media upload to be multipart form with specific parameters
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async ValueTask AppendMediaAsync(string mediaId, int segmentIndex, byte[] payload)
        {
            await Task.CompletedTask;
            /*
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            const string uploadUrl = TwitterApi.UploadMediaUrl;
            var signature = OAuth.Signature(POST, uploadUrl, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, parameters: null);
            var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);

            var request = WebRequest.Create(uploadUrl);
            request.Headers.Add("Authorization", authorizeHeader);
            request.Method = POST;

            var boundary = $"{Guid.NewGuid():N}";
            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
            await TextParameterAsync(requestStream, boundary, "command", "APPEND").ConfigureAwait(false);
            await TextParameterAsync(requestStream, boundary, "media_id", mediaId).ConfigureAwait(false);
            await TextParameterAsync(requestStream, boundary, "segment_index", segmentIndex.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await BinaryParameterAsync(requestStream, boundary, "media", payload).ConfigureAwait(false);
            await WriteTextToStreamAsync(requestStream, $"--{boundary}--\r\n").ConfigureAwait(false);

            using var _ = await request.GetResponseAsync().ConfigureAwait(false);
            */
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