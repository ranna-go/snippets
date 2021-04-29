using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class SnippetRequest
    {
        [JsonPropertyName("language")]
        [MaxLength(50)]
        [RegularExpression(@"^[\w\-_:\.]+$")]
        [Required]
        public string Language { get; set; }

        [JsonPropertyName("code")]
        [MaxLength(500_000)]
        [Required]
        public string Code { get; set; }
    }
}
