using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class Variant
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}