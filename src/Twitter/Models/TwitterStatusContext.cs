using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Twitter.Models
{
    [JsonSerializable(typeof(IEnumerable<TwitterStatus>))]
    public partial class TwitterStatusContext : JsonSerializerContext
    {
    }
}