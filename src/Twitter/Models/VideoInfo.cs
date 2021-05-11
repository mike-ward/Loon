using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class VideoInfo
    {
        [JsonPropertyName("variants")] public Variant[]? Variants { get; set; }
    }
}