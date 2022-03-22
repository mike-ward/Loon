using System;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class Tweet
    {
        [JsonPropertyName("statuses")]
        public TwitterStatus[] Statuses { get; set; } = Array.Empty<TwitterStatus>();
    }
}