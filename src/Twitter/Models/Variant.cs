using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class Variant
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}