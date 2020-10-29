using System;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Entity
{
    public class EntityMapping
    {
        private EntityMapping(Type @interface, Type implementation)
        {
            Interface = @interface;
            Implementation = implementation;
        }

        public Type Interface { get; }
        public Type Implementation { get; }

        public class TypedEntityMapping<TInterface, TImplementation> : EntityMapping
            where TInterface : IEntity
            where TImplementation : Entity, TInterface
        {
            public TypedEntityMapping()
                : base(typeof(TInterface), typeof(TImplementation))
            {
            }
        }
    }
}
