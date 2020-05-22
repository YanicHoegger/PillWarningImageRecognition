using DatabaseInteraction.Interface;
using Newtonsoft.Json;
using System;

namespace DatabaseInteraction
{
    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public sealed override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
