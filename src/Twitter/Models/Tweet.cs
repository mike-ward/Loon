using System;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class Tweet
    {
        [JsonPropertyName("statuses")] public TwitterStatus[] Statuses { get; set; } = Array.Empty<TwitterStatus>();
    }
}