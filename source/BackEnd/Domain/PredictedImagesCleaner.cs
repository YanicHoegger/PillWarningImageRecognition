using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Domain.Interface;
using ImageInteraction.Interface;

namespace Domain
{
    public class PredictedImagesCleaner : IPredictedImagesCleaner
    {
        private readonly IDrugCheckingSourceRepository _repository;
        private readonly IPredictedImagesManager _predictedImagesManager;

        public PredictedImagesCleaner(IDrugCheckingSourceRepository repository, IPredictedImagesManager predictedImagesManager)
        {
            _repository = repository;
            _predictedImagesManager = predictedImagesManager;
        }

        public async Task CleanPredictions()
        {
            await foreach (var drugCheckingSource in _repository.Get())
            {
                await _predictedImagesManager.DeletePredictedImages(drugCheckingSource.Image);
            }
        }
    }
}
