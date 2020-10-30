using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageInteraction.Interface;
using Utilities;

namespace ImageInteraction.PredictedImagesManager
{
    public class PredictedImagesManager : IPredictedImagesManager
    {
        private readonly IContext _context;
        private readonly IPredictedImagesProvider _predictedImagesProviderBase;
        private readonly HttpClient _client;

        public PredictedImagesManager(IContext context, IPredictedImagesProvider predictedImagesProviderBase)
        {
            _context = context;
            _predictedImagesProviderBase = predictedImagesProviderBase;
            _client = ClientFactory.Create(context.Key);
        }

        public async Task DeletePredictedImage(byte[] image)
        {
            var sameImages = await _predictedImagesProviderBase.GetPredictedImages()
                .Where(x => ImageHelper.Compare(x.Image, image))
                .Select(x => x.Id)
                .ToListAsync();

            await DeleteImages(sameImages);
        }

        public IAsyncEnumerable<IPredictedImage> GetPredictedImages()
        {
            return _predictedImagesProviderBase.GetPredictedImages();
        }

        private async Task DeleteImages(IEnumerable<string> predictedImages)
        {
            var predictedImagesList = predictedImages.ToList();
            if (!predictedImagesList.Any())
            {
                return;
            }

            var response = await _client.DeleteAsync(GetDeleteUri(predictedImagesList));
            response.EnsureSuccessStatusCode();
        }

        private string GetDeleteUri(IEnumerable<string> ids)
        {
            var concatenatedIds = string.Join("&", ids.Select(x => $"ids={x}"));
            return $"{_context.EndPoint}/customvision/v3.0/training/projects/{_context.ProjectId}/predictions?{concatenatedIds}";
        }
    }
}
