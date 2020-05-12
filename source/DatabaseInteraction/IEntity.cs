using Newtonsoft.Json;
using System;

namespace DatabaseInteraction
{
    public interface IEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
    }
}
