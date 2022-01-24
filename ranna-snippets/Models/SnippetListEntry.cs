using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class SnippetListEntry : Entity
    {
        [JsonPropertyName("ident")]
        public string Ident { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("displayname")]
        public string Displayname { get; set; }

        public SnippetListEntry()
        { }

        public SnippetListEntry(Snippet s)
        {
            Id = s.Id;
            Displayname = s.Displayname;
            TimeStamp = s.TimeStamp;
            Ident = s.Ident;
            Language = s.Language;
        }
    }
}
