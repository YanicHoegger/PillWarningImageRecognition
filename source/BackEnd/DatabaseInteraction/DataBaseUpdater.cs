using DatabaseInteraction.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task Update<T>(IRepositoryFactory repositoryFactory, IEnumerable<T> toUpdate, Func<T, bool> comparer) 
            where T : Entity, new()
        {
            var repository = repositoryFactory.Create<T>();

            var items = await repository.Get();

            foreach(var update in toUpdate)
            {
                var correspondingItem = items.SingleOrDefault(comparer);
                if(correspondingItem == null)
                {
                    _logger.LogWarning($"Could not find item:\r\n{JsonSerializer.Serialize(update)}");
                    continue;
                }

                await repository.Update(update, correspondingItem.Id);
            }
        }
    }
}
