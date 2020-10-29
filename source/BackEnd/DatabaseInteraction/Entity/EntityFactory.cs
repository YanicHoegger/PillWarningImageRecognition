using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseInteraction.Interface;
using Utilities;

namespace DatabaseInteraction.Entity
{
    public class EntityFactory : IEntityFactory
    {
        private readonly Dictionary<Type, Type> _mappings;

        public EntityFactory(IEnumerable<EntityMapping> entityMappings)
        {
            _mappings = entityMappings.ToDictionary(x => x.Interface, x => x.Implementation);
        }

        public T Create<T>() where T : IEntity
        {
            var instance = (T)Activator.CreateInstance(_mappings[typeof(T)]);

            if(instance == null)
                throw new InvalidOperationException($"Could not create instance of {_mappings[typeof(T)]} for {typeof(T)}");

            // ReSharper disable once PossibleNullReferenceException
            (instance as Entity).Id = Guid.NewGuid();

            return instance;
        }

        public TImplementation CreateImplementation<TInterface, TImplementation>(TInterface from)
        {
            return (TImplementation)CreateImplementation(typeof(TInterface), typeof(TImplementation), from);
        }

        private object CreateImplementation(Type @interface, Type implementation, object from)
        {
            var newInstance = Activator.CreateInstance(implementation);

            DeepCopyValues(from, newInstance, @interface);

            return newInstance;
        }

        private void DeepCopyValues(object from, object to, Type fromType)
        {
            var fromPropertyInfos = fromType.GetPublicProperties();
            var toPropertyInfos = to.GetType().GetPublicProperties().ToList();

            foreach (var propertyInfo in fromPropertyInfos)
            {
                var toPropertyInfo = toPropertyInfos.Single(x => x.Name.Equals(propertyInfo.Name));

                if (typeof(IEntity).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var value = propertyInfo.GetValue(from);
                    var newValue = CreateImplementation(propertyInfo.PropertyType, _mappings[propertyInfo.PropertyType], value);

                    toPropertyInfo.SetValue(to, newValue);
                }
                else if (typeof(IEnumerable<IEntity>).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var value = (IEnumerable<IEntity>)propertyInfo.GetValue(from);

                    if(value == null)
                        continue;

                    var entityType = propertyInfo.PropertyType.GenericTypeArguments.First();

                    var newValue = value.Select(x => CreateImplementation(entityType, _mappings[entityType], x));
                    // ReSharper disable once PossibleNullReferenceException
                    var castedNewValue = typeof(Enumerable)
                        .GetMethod(nameof(Enumerable.Cast))
                        .MakeGenericMethod(entityType)
                        .Invoke(null, new object[] {newValue});

                    toPropertyInfo.SetValue(to, castedNewValue);
                }
                else if (toPropertyInfo.CanWrite)
                {
                    var value = propertyInfo.GetValue(from);
                    toPropertyInfo.SetValue(to, value);
                }
            }
        }
    }
}
