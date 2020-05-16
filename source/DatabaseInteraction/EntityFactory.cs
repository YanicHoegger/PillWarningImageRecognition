using DatabaseInteraction.Interface;
using System;

namespace DatabaseInteraction
{
    public class EntityFactory : IEntityFactory
    {
        public T Create<T>() where T : Entity, new()
        {
            var instance = new T
            {
                Id = Guid.NewGuid()
            };

            return instance;
        }
    }
}
