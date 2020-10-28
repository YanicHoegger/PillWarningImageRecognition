using System.Net.Http;
using System.Net.Http.Headers;

namespace ImageInteraction.PredictedImagesManager
{
    public static class ClientFactory
    {
        public static HttpClient Create(string key)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Training-Key", new[] { key });

            return client;
        }
    }
}
