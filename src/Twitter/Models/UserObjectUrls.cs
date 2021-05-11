using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class UserObjectUrls
    {
        [JsonPropertyName("urls")] public UrlEntity[]? Urls { get; set; }
    }
}