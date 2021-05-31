using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class ErrorModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
