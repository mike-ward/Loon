﻿using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Loon.Extensions;

namespace Loon.Services
{
    internal static class TranslateService
    {
        private const string Endpoint = "https://libretranslate.com/translate";

        public static async ValueTask<string> Translate(string? text, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "no source text to translate";
            }

            try
            {
                var request = WebRequest.Create(Endpoint);

                var q = "q=" + Uri.EscapeDataString(text);
                var s = "&source=" + Uri.EscapeDataString(fromLanguage);
                var t = "&target=" + Uri.EscapeDataString(toLanguage);
                var bytes = Encoding.UTF8.GetBytes(q + s + t);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;

                var content = await request.GetRequestStreamAsync().ConfigureAwait(false);
                await content.WriteAsync(bytes).ConfigureAwait(false);
                content.Close();

                using var response = await request.GetResponseAsync().ConfigureAwait(false);
                await using var stream = response.GetResponseStream();
                var result = await JsonSerializer.DeserializeAsync<TranslatorResult>(stream).ConfigureAwait(false);
                return result?.TranslatedText?.HtmlDecode() ?? "{error}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class TranslatorResult
    {
        [JsonPropertyName("translatedText")]
        public string? TranslatedText { get; set; }
    }
}