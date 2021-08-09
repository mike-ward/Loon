using System;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class Media
    {
        [JsonPropertyName("url")]
        public string Url { get; init; } = string.Empty;

        [JsonPropertyName("display_url")]
        public string DisplayUrl { get; init; } = string.Empty;

        [JsonPropertyName("expanded_url")]
        public string ExpandedUrl { get; init; } = string.Empty;

        [JsonPropertyName("media_url")]
        public string MediaUrl { get; init; } = string.Empty;

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; } = Array.Empty<int>();

        [JsonPropertyName("video_info")]
        public VideoInfo? VideoInfo { get; set; }
    }
}