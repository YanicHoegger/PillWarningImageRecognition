using Newtonsoft.Json;
using System;

namespace DatabaseInteraction
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }

        public sealed override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
