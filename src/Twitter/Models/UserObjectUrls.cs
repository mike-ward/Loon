using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class UserObjectUrls
    {
        [JsonPropertyName("urls")]
        public UrlEntity[]? Urls { get; set; }
    }
}