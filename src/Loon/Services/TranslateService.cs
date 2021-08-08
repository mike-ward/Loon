using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Jab;
using Loon.Extensions;
using Loon.Interfaces;

namespace Loon.Services
{
    internal static class TranslateService
    {
        private static readonly JabServiceProvider serviceProvider;

        static TranslateService()
        {
            serviceProvider = Bootstrapper.ServiceProvider;
        }

        private const string Endpoint = "https://libretranslate.com/translate";

        public static async ValueTask<string> Translate(string? text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "no source text to translate";
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
                var apiKey  = serviceProvider.GetService<ISettings>().TranslateApiKey ?? string.Empty;

                var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string?, string?>("q", Uri.EscapeDataString(text)),
                    new KeyValuePair<string?, string?>("source", Uri.EscapeDataString(fromLanguage)),
                    new KeyValuePair<string?, string?>("target", Uri.EscapeDataString(toLanguage)),
                    new KeyValuePair<string?, string?>("apiKey", Uri.EscapeDataString(apiKey))
                });

                using var response = await Twitter.Services.OAuthApiRequest.MyHttpClient.SendAsync(request).ConfigureAwait(false);
                var       stream   = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var       result   = await JsonSerializer.DeserializeAsync<TranslatorResult>(stream).ConfigureAwait(false);
                return result?.TranslatedText?.HtmlDecode() ?? result?.ErrorText?.HtmlDecode() ?? "{error}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class TranslatorResult
    {
        [JsonPropertyName("translatedText")] public string? TranslatedText { get; set; }
        [JsonPropertyName("error")]          public string? ErrorText      { get; set; }
    }
}