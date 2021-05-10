using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class User : Entity
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonIgnore]
        public string MasterKeyHash { get; set; }

        [JsonIgnore]
        public string ApiKey { get; set; }
    }
}
