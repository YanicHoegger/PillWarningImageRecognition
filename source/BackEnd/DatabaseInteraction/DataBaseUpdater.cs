using DatabaseInteraction.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public class DataBaseUpdater : IDataBaseUpdater
    {
        private readonly ILogger<DataBaseUpdater> _logger;

        public DataBaseUpdater(ILogger<DataBaseUpdater> logger)
        {
            _logger = logger;
        }

        public async Task Update<T>(IRepository<T> repository, T toUpdate, Func<T, bool> predicate) 
            where T : Entity, new()
        {
            var items = await repository.Get();

            var correspondingItem = items.SingleOrDefault(predicate);
            if (correspondingItem == null)
            {
                _logger.LogWarning($"Could not find item:\r\n{JsonSerializer.Serialize(toUpdate)}");
                return;
            }

            await repository.Update(toUpdate, correspondingItem.Id);
        }
    }
}
