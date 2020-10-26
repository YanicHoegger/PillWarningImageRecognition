using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Domain.Interface;
using ImageInteraction.Interface;

namespace Domain
{
    public class PredictedImagesCleaner : IPredictedImagesCleaner
    {
        private readonly IRepository<DrugCheckingSource> _repository;
        private readonly IPredictedImagesManager _predictedImagesManager;

        public PredictedImagesCleaner(IRepository<DrugCheckingSource> repository, IPredictedImagesManager predictedImagesManager)
        {
            _repository = repository;
            _predictedImagesManager = predictedImagesManager;
        }

        public async Task CleanPredictions()
        {
            foreach (var drugCheckingSource in await _repository.Get())
            {
                await _predictedImagesManager.DeletePredictedImages(drugCheckingSource.Image);
            }
        }
    }
}
