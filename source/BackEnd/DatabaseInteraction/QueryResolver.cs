using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace DatabaseInteraction
{
    public static class QueryResolver
    {
        public static QueryDefinition SelectOfType<T>()
        {
            var propertyNames = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(GetPropertyNameOnDb);

            var query = $"SELECT * FROM c WHERE {string.Join(" AND ", propertyNames.Select(x => $"c.{x} <> null"))}";

            return new QueryDefinition(query);
        }

        private static string GetPropertyNameOnDb(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes<JsonPropertyAttribute>(true);
            var propertyName = attributes.Select(x => x.PropertyName).FirstOrDefault(x => !string.IsNullOrEmpty(x));

            if (propertyName != null)
                return propertyName;

            var interfaces = propertyInfo.DeclaringType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                var property = @interface.GetProperty(propertyInfo.Name);
                if (property == null)
                    continue;

                var fromInterface = GetPropertyNameOnDb(property);

                if (!fromInterface.Equals(propertyInfo.Name))
                    return fromInterface;
            }

            return propertyInfo.Name;
        }
    }
}
