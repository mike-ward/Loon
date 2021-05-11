using System;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class MentionEntity
    {
        [JsonPropertyName("id_str")] public string Id { get; set; } = string.Empty;

        [JsonPropertyName("screen_name")] public string ScreenName { get; set; } = string.Empty;

        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

        [JsonPropertyName("indices")] public int[] Indices { get; set; } = Array.Empty<int>();
    }
}