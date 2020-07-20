using Microsoft.Extensions.Configuration;
using System;

namespace Utilities
{
    public static class ConfigurationExtensions
    {
        public static string GetNotEmptyValue(this IConfiguration configuration, string key)
        {
            var value = configuration[key];

            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"Configuration for {key} is not set");

            return value;
        }

        public static bool ReadBool(this IConfiguration configuration, string key)
        {
            var couldParse = bool.TryParse(configuration[key], out var readValue);

            return couldParse && readValue;
        }
    }
}
