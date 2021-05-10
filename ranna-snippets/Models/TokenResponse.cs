using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
