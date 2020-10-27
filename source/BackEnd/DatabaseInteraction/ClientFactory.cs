using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction
{
    public static class ClientFactory
    {
        public static CosmosClient Create(IContext context)
        {
            return new CosmosClient(context.EndPoint, context.Key, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway
            });
        }
    }
}
