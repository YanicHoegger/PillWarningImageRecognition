using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebInterface.Shared
{
    public static class HttpClientExtensions
    {
        //TODO: Move awai from Utilities --> There should be utitilities for back end and for clients
        public static async Task<T> PostImageAsync<T>(this HttpClient httpClient, Stream image, string fileName, string uri)
        {
            var fileStreamContent = new StreamContent(image);

            fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = fileName
            };

            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using var formData = new MultipartFormDataContent
            {
                fileStreamContent
            };

            var response = await httpClient.PostAsync(uri, formData);

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
