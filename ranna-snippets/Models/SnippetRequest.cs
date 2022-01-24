using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class SnippetRequest
    {
        [JsonPropertyName("language")]
        [MaxLength(50)]
        [RegularExpression(@"^[\w\-_:\.]+$")]
        public string Language { get; set; }

        [JsonPropertyName("code")]
        [MaxLength(500_000)]
        public string Code { get; set; }

        [JsonPropertyName("displayname")]
        [MaxLength(512)]
        public string Displayname { get; set; }
    }
}
