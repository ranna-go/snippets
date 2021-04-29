using System;
using System.Text;
using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class Snippet : Entity
    {
        [JsonPropertyName("ident")]
        public string Ident { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        public Snippet() : base()
        { }

        public bool Validate(out string errMsg)
        {
            if (string.IsNullOrWhiteSpace(Language))
            {
                errMsg = "empty language";
                return false;
            }
            if (string.IsNullOrEmpty(Code))
            {
                errMsg = "empty code";
                return false;
            }

            errMsg = null;
            return true;
        }

        public Snippet Encode()
        {
            Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(Code));
            return this;
        }

        public Snippet Decode()
        {
            Code = Encoding.UTF8.GetString(Convert.FromBase64String(Code));
            return this;
        }
    }
}
