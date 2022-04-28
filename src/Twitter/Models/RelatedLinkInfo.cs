using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Twitter.Services;

namespace Twitter.Models
{
    /// <summary>
    ///     When links are included in a tweet and it's not a quote or retweet,
    ///     look for og/twitter meta tags in the links. This info, if present,
    ///     is displayed similar to quotes.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class RelatedLinkInfo
    {
        public string  Url         { get; private set; } = string.Empty;
        public string  Title       { get; private set; } = string.Empty;
        public string? ImageUrl    { get; private set; }
        public string  Description { get; private set; } = string.Empty;
        public string  SiteName    { get; private set; } = string.Empty;
        public string  Language    { get; private set; } = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        public TwitterStatus ImageTwitterStatus =>
            new()
            {
                Id       = Guid.NewGuid().ToString(),
                Language = Language,
                FullText = Description,
                ExtendedEntities = new Entities
                {
                    Media = string.IsNullOrWhiteSpace(ImageUrl)
                        ? null
                        : new[]
                        {
                            new Media
                            {
                                Url         = ImageUrl,
                                MediaUrl    = ImageUrl,
                                DisplayUrl  = ImageUrl,
                                ExpandedUrl = ImageUrl
                            }
                        }
                }
            };

        public static async ValueTask<RelatedLinkInfo?> GetRelatedLinkInfoAsync(TwitterStatus status, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return status.RelatedLinkInfo;
            if (status.IsQuoted) return status.RelatedLinkInfo;

            var urls = status.Entities?.Urls;
            if (urls is null) return status.RelatedLinkInfo;

            var hasMedia = status.OriginatingStatus.ExtendedEntities?.HasMedia ?? false;

            foreach (var url in urls)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    var uri = url.ExpandedUrl ?? url.Url;
                    if (!UrlValid(uri)) continue;
                    var relatedLinkInfo = await GetLinkInfoAsync(uri, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) break;
                    if (relatedLinkInfo is null) continue;

                    if (hasMedia || !UrlValid(relatedLinkInfo.ImageUrl))
                    {
                        relatedLinkInfo.ImageUrl = null;
                    }

                    return status.RelatedLinkInfo ?? relatedLinkInfo;
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (HttpRequestException)
                {
                    // eat it
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }

            return status.RelatedLinkInfo;
        }

        private static async ValueTask<RelatedLinkInfo?> GetLinkInfoAsync(string url, CancellationToken cancellationToken)
        {
            using var response = await OAuthApiRequest.MyHttpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) return null;

            using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false), Encoding.UTF8);
            if (cancellationToken.IsCancellationRequested) return null;

            var htmlBuilder = new StringBuilder();

            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return null;
                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line is null || cancellationToken.IsCancellationRequested) return null;
                htmlBuilder.AppendLine(line);

                // No need to parse the whole document, only interested in head section
                const string headCloseTag = "</head>";
                if (line.Contains(headCloseTag, StringComparison.OrdinalIgnoreCase)) break;
            }

            var metaInfo = ParseForSocialTags(url, $"{htmlBuilder}</html>", cancellationToken);

            return !string.IsNullOrEmpty(metaInfo?.Title) && !string.IsNullOrEmpty(metaInfo.Description)
                ? metaInfo
                : null;
        }

        private static RelatedLinkInfo? ParseForSocialTags(string url, string html, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            var document = new HtmlDocument();
            document.LoadHtml(html);
            if (cancellationToken.IsCancellationRequested) return null;

            var language = document.DocumentNode.SelectSingleNode("//html")?.Attributes["lang"]?.Value;
            if (cancellationToken.IsCancellationRequested) return null;

            if (string.IsNullOrWhiteSpace(language) || language.Equals("und", StringComparison.OrdinalIgnoreCase))
            {
                language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }

            var metaTags = document.DocumentNode.SelectNodes("//meta");
            if (cancellationToken.IsCancellationRequested) return null;
            var metaInfo = new RelatedLinkInfo { Url = url, Language = Truncate(language, 2) };

            if (metaTags is not null)
            {
                foreach (var tag in metaTags)
                {
                    if (cancellationToken.IsCancellationRequested) return null;

                    var tagName     = tag.Attributes["name"];
                    var tagContent  = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];

                    if (tagName is not null && tagContent is not null)
                    {
                        switch (tagName.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "title":
                                metaInfo.Title = DecodeHtml(tagContent.Value);
                                break;

                            case "description":
                                metaInfo.Description = DecodeHtml(tagContent.Value);
                                break;

                            case "twitter:title":
                                metaInfo.Title = DecodeHtml(tagContent.Value);
                                break;

                            case "twitter:description":
                                metaInfo.Description = DecodeHtml(tagContent.Value);
                                break;

                            case "twitter:image:src":
                                metaInfo.ImageUrl = tagContent.Value;
                                break;

                            case "twitter:site":
                                metaInfo.SiteName = DecodeHtml(tagContent.Value);
                                break;
                        }
                    }
                    else if (tagProperty is not null && tagContent is not null)
                    {
                        switch (tagProperty.Value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "og:title":
                                metaInfo.Title = DecodeHtml(tagContent.Value);
                                break;

                            case "og:description":
                                metaInfo.Description = DecodeHtml(tagContent.Value);
                                break;

                            case "og:image":
                                metaInfo.ImageUrl = tagContent.Value;
                                break;

                            case "og:site_name":
                                metaInfo.SiteName = DecodeHtml(tagContent.Value);
                                break;
                        }
                    }
                }
            }

            return metaInfo;
        }

        private static bool UrlValid(string? source)
        {
            if (string.IsNullOrWhiteSpace(source)) return false;

            try
            {
                var url = new Uri(source);
                return url.IsWellFormedOriginalString();
            }
            catch
            {
                return false;
            }
        }

        private static string DecodeHtml(string text)
        {
            // Twice to handle sequences like: "&amp;mdash;"
            return WebUtility.HtmlDecode(WebUtility.HtmlDecode(text)) ?? string.Empty;
        }

        private static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }

            return source;
        }
    }
}