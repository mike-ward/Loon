using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class VideoInfo
    {
        [JsonPropertyName("variants")]
        public Variant[]? Variants { get; set; }
    }
}