using DatabaseInteraction.Interface;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Domain
{
    public class DrugCheckingSourceHandler
    {
        private readonly IDrugCheckingSourceRepository _repository;
        private readonly ILogger<DrugCheckingSourceHandler> _logger;

        public DrugCheckingSourceHandler(IDrugCheckingSourceRepository repository, ILogger<DrugCheckingSourceHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task StoreSources(DrugCheckingSource source)
        {
            if (await _repository.Contains(source))
            {
                return;
            }

            await _repository.Insert(source);
        }

        public async Task UpdateResources(DrugCheckingSource toUpdate)
        {
            var correspondingItem = await _repository.SingleOrDefault(toUpdate);
            if (correspondingItem == null)
            {
                _logger.LogWarning($"Could not find item:\r\n{toUpdate.PdfLocation}");
                return;
            }

            await _repository.Update(toUpdate, correspondingItem.Id);
        }
    }
}
