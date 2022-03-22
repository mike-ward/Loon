using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class CurrentUserRetweet
    {
        [JsonPropertyName("id_str")]
        public string? Id { get; set; }
    }
}