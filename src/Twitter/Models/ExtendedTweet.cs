﻿using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public sealed class ExtendedTweet
    {
        [JsonPropertyName("full_text")]
        public string? FullText { get; set; }

        [JsonPropertyName("entities")]
        public Entities? Entities { get; set; }

        [JsonPropertyName("extended_entities")]
        public Entities? ExtendedEntities { get; set; }
    }
}