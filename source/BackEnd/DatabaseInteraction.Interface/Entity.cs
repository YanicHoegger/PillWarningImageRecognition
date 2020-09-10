using System;
using Newtonsoft.Json;

namespace DatabaseInteraction.Interface
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
