using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class UserCreateResponse : Entity
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("masterkey")]
        public string MasterKey { get; set; }
    }
}
