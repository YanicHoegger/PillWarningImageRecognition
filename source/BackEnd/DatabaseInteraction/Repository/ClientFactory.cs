using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    public class ClientFactory
    {
        private readonly JsonCosmosSerializer _jsonCosmosSerializer;

        public ClientFactory(JsonCosmosSerializer jsonCosmosSerializer)
        {
            _jsonCosmosSerializer = jsonCosmosSerializer;
        }

        public CosmosClient Create(IContext context)
        {
            return new CosmosClient(context.EndPoint, context.Key, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway,
                Serializer = _jsonCosmosSerializer
            });
        }
    }
}
