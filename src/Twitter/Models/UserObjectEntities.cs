using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class UserObjectEntities
    {
        [JsonPropertyName("url")]
        public UserObjectUrls? Url { get; set; }
    }
}