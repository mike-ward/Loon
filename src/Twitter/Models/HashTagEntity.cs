using System;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class HashTagEntity
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("indices")]
        public int[] Indices { get; set; } = Array.Empty<int>();
    }
}