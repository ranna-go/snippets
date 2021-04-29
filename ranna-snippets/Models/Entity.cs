using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ranna_snippets.Models
{
    public class Entity
    {
        [Key]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset TimeStamp { get; set; }

        public Entity()
        {
            Id = new Guid();
            TimeStamp = DateTimeOffset.Now;
        }
    }
}
