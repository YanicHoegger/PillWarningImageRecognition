using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Microsoft.Extensions.Logging;

namespace Domain.DrugCheckingSource
{
    public class StorageHandler
    {
        private readonly IDrugCheckingSourceRepository _repository;
        private readonly ILogger<StorageHandler> _logger;

        public StorageHandler(IDrugCheckingSourceRepository repository, ILogger<StorageHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task StoreSources(IDrugCheckingSource source)
        {
            if (await _repository.Contains(source))
            {
                return;
            }

            await _repository.Insert(source);
        }

        public async Task UpdateResources(IDrugCheckingSource toUpdate)
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
