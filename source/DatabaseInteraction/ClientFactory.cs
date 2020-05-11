using MongoDB.Driver;
using System.Security.Authentication;

namespace DatabaseInteraction
{
    public static class ClientFactory
    {
        public static MongoClient Create(IContext context)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(context.ConnectionString));

            settings.SslSettings = new SslSettings
            { 
                EnabledSslProtocols = SslProtocols.Tls12 
            };

            return new MongoClient(settings);
        }
    }
}
