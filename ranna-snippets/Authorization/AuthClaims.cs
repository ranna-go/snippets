using ranna_snippets.Models;
using System;
using System.Text.Json.Serialization;

namespace ranna_snippets.Authorization
{
    public class AuthClaims
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("userid")]
        public Guid UserUid { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
