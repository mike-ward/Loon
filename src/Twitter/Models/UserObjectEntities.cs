using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class UserObjectEntities
    {
        [JsonPropertyName("url")] public UserObjectUrls? Url { get; set; }
    }
}