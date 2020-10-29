using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DatabaseInteraction.Entity;
using Microsoft.Azure.Cosmos;
using Utilities;

namespace DatabaseInteraction.Repository
{
    public class JsonCosmosSerializer : CosmosSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonCosmosSerializer(IEnumerable<EntityMapping> entityMappings)
        {
            _jsonSerializerOptions = CreateJsonSerializerOptions(entityMappings);
        }

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                return JsonSerializer.DeserializeAsync<T>(stream, _jsonSerializerOptions).Result;
            }
        }

        public override Stream ToStream<T>(T input)
        {
            MemoryStream streamPayload = new MemoryStream();

            JsonSerializer.SerializeAsync(streamPayload, input).Wait();

            streamPayload.Position = 0;

            return streamPayload;
        }

        private static JsonSerializerOptions CreateJsonSerializerOptions(IEnumerable<EntityMapping> entityMappings)
        {
            var converterFactory = new InterfaceImplementationJsonConverterFactory();

            foreach (var entityMapping in entityMappings)
            {
                converterFactory.Register(entityMapping.Interface, entityMapping.Implementation);
            }

            return new JsonSerializerOptions
            {
                Converters = { converterFactory }
            };
        }
    }
}
