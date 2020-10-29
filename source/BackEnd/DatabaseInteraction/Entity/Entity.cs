using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Entity
{
    public class Entity : IEntity
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        public sealed override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
