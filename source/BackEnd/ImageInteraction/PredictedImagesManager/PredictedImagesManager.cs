using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ImageInteraction.Classification;
using ImageInteraction.Interface;
using Microsoft.Extensions.Hosting;

namespace ImageInteraction.PredictedImagesManager
{
    //TODO only configure when used in client, but then use IHostedService to fetch data and cache them
    public class PredictedImagesManager : IPredictedImagesManager, IHostedService
    {
        private readonly IContext _context;
        private readonly HttpClient _client;

        private readonly List<PredictedImage> _predictedImages = new List<PredictedImage>();

        public PredictedImagesManager(IContext context)
        {
            _context = context;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Training-Key", new[] { _context.Key });
        }

        public async Task DeletePredictedImage(IEnumerable<byte[]> images)
        {
            var sameImages = _predictedImages.Where(x => images.Contains(x.Image));

            await DeleteImages(sameImages);
        }

        public IEnumerable<IPredictedImage> GetPredictedImages()
        {
            return _predictedImages;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _predictedImages.Clear();
            _predictedImages.AddRange(await GetPredictedImagesInternal(cancellationToken).ToListAsync(cancellationToken));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task DeleteImages(IEnumerable<PredictedImage> predictedImages)
        {
            var predictedImagesList = predictedImages.ToList();
            if (!predictedImagesList.Any())
            {
                return;
            }

            var response = await _client.DeleteAsync(GetDeleteUri(predictedImagesList.Select(x => x.Id)));
            response.EnsureSuccessStatusCode();
        }

        private string GetDeleteUri(IEnumerable<string> ids)
        {
            var concatenatedIds = string.Join("&", ids.Select(x => $"ids={x}"));
            return $"{_context.EndPoint}/customvision/v3.0/training/projects/{_context.ProjectId}/predictions?{concatenatedIds}";
        }

        private async IAsyncEnumerable<PredictedImage> GetPredictedImagesInternal([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var serialized = await ReadPredictedImages(cancellationToken);

            await foreach (var converted in Convert(serialized, cancellationToken))
            {
                yield return converted;
            }
        }

        private async Task<PredictedImagesDto> ReadPredictedImages(CancellationToken cancellationToken)
        {
            var response = await _client.PostAsync(GetPostUrl(), GetHttpContent(), cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return await JsonSerializer.DeserializeAsync<PredictedImagesDto>(content, serializeOptions, cancellationToken);
        }

        private string GetPostUrl()
        {
            return $"{_context.EndPoint}/customvision/v3.0/training/projects/{_context.ProjectId}/predictions/query";
        }

        private static HttpContent GetHttpContent()
        {
            return new StringContent("{ \"orderBy\": \"Newest\", \"maxCount\": \"1000\" }", Encoding.UTF8, "application/json");
        }

        private async IAsyncEnumerable<PredictedImage> Convert(PredictedImagesDto predictedImagesDto, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var predictedImageDto in predictedImagesDto.Results)
            {
                var image = await ReadImage(predictedImageDto.OriginalImageUri, cancellationToken);
                var tagClassificationResults = predictedImageDto.Predictions.Select(Convert).ToList();

                yield return new PredictedImage(tagClassificationResults, image, predictedImageDto.Id);
            }
        }

        private async Task<byte[]> ReadImage(string url, CancellationToken cancellationToken)
        {
            var imageStream = await _client.GetStreamAsync(url);

            var memoryStream = new MemoryStream();
            await imageStream.CopyToAsync(memoryStream, cancellationToken);

            return memoryStream.ToArray();
        }

        private static ITagClassificationResult Convert(PredictionResultDto predictionResultDto)
        {
            return new TagClassificationResult(predictionResultDto.TagName, predictionResultDto.Probability);
        }
    }
}
