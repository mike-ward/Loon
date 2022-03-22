using System;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class UrlEntity
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("display_url")]
        public string DisplayUrl { get; set; } = string.Empty;

        [JsonPropertyName("expanded_url")]
        public string ExpandedUrl { get; set; } = string.Empty;

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; } = Array.Empty<int>();
    }
}