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
using ImageInteraction.PredictedImagesManager.Dtos;
using Microsoft.Extensions.Hosting;
using Utilities;

namespace ImageInteraction.PredictedImagesManager
{
    public class PredictedImagesManager : IPredictedImagesManager, IHostedService
    {
        private const int _maxCount = 128;

        private readonly IContext _context;
        private readonly HttpClient _client;

        private readonly List<PredictedImage> _predictedImages = new List<PredictedImage>();

        public PredictedImagesManager(IContext context)
        {
            _context = context;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Training-Key", new[] { _context.Key });
        }

        public async Task DeletePredictedImages(byte[] image)
        {
            var sameImages = _predictedImages.Where(x => ImageHelper.Compare(x.Image, image));

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

        private IAsyncEnumerable<PredictedImage> GetPredictedImagesInternal(CancellationToken cancellationToken)
        {
            return ReadPredictedImages(cancellationToken)
                .SelectAwait(async predictedImages => await Convert(predictedImages, cancellationToken));
        }

        private async IAsyncEnumerable<PredictedImageDto> ReadPredictedImages([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            PredictedImagesDto result = null;
            do
            {
                result = await ReadPredictedImages(result, cancellationToken);

                foreach (var predictedImageDto in result.Results)
                {
                    yield return predictedImageDto;
                }
            } while (result.Results.Count >= _maxCount);
        }

        private async Task<PredictedImagesDto> ReadPredictedImages(PredictedImagesDto predictedImagesDto, CancellationToken cancellationToken)
        {
            var token = predictedImagesDto?.Token ?? new TokenDto
            {
                MaxCount = _maxCount,
                OrderBy = "Newest"
            };

            var response = await _client.PostAsync(GetPostUrl(), GetHttpContent(token), cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();

            return await Deserialize(cancellationToken, content);
        }

        private static async Task<PredictedImagesDto> Deserialize(CancellationToken cancellationToken, Stream content)
        {
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

        private static HttpContent GetHttpContent(TokenDto dto)
        {
            return new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
        }

        private async Task<PredictedImage> Convert(PredictedImageDto predictedImageDto, CancellationToken cancellationToken)
        {
            var image = await ReadImage(predictedImageDto.OriginalImageUri, cancellationToken);
            var tagClassificationResults = predictedImageDto.Predictions.Select(Convert).ToList();

            return new PredictedImage(tagClassificationResults, image, predictedImageDto.Id);
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
