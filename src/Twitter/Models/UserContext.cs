using System.Text.Json.Serialization;

namespace Twitter.Models
{
    [JsonSerializable(typeof(User))]
    public partial class UserContext : JsonSerializerContext
    {
    }
}