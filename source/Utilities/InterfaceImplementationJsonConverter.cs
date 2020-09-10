﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities
{
    public class InterfaceImplementationJsonConverter<TInterface, TImplementation> : JsonConverter<TInterface>
        where TImplementation : TInterface
    {
        public override TInterface Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TImplementation>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
