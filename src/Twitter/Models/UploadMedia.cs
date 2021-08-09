﻿using System.Text.Json.Serialization;

namespace Twitter.Models
{
    public class UploadMedia
    {
        [JsonPropertyName("media_id_string")]
        public string MediaId { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("expires_after_secs")]
        public int ExpiresAfterSecs { get; set; }

        [JsonPropertyName("processing_info")]
        public ProcessingInfo ProcessingInfo { get; set; } = ProcessingInfo.Empty;
    }
}