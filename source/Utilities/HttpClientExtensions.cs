using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task<T> PostImageAsync<T>([NotNull] this HttpClient httpClient, [NotNull] Stream image, [NotNull] string fileName, [NotNull] string uri)
        {
            httpClient.CheckNotNull(nameof(httpClient));
            image.CheckNotNull(nameof(image));
            fileName.CheckNotNullOrEmpty(nameof(fileName));
            uri.CheckNotNullOrEmpty(nameof(uri));

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

        public static async Task<T> GetAsync<T>([NotNull] this HttpClient httpClient, [NotNull] string uri)
        {
            httpClient.CheckNotNull(nameof(httpClient));
            uri.CheckNotNullOrEmpty(nameof(uri));

            var response = await httpClient.GetAsync(uri);

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
