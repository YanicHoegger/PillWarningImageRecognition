using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities
{
    public class InterfaceImplementationJsonConverterFactory : JsonConverterFactory
    {
        private readonly Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();

        public override bool CanConvert(Type typeToConvert)
        {
            return _registrations.ContainsKey(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (!_registrations.ContainsKey(typeToConvert))
            {
                throw new InvalidOperationException($"There is no registration for type {typeToConvert}");
            }

            var converterType = typeof(InterfaceImplementationJsonConverter<,>).MakeGenericType(typeToConvert, _registrations[typeToConvert]);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        public void Register<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            Register(typeof(TInterface), typeof(TImplementation));
        }

        public void Register(Type interfaceType, Type implementationType)
        {
            _registrations[interfaceType] = implementationType;
        }
    }
}
