using System.Threading.Tasks;
using DrugCheckingCrawler.Interface;

namespace Domain.DrugCheckingSource
{
    public class Updater
    {
        private readonly StorageHandler _storageHandler;
        private readonly IResourceCrawler _resourceCrawler;
        private readonly IFactory _drugCheckingSourceFactory;

        public Updater(StorageHandler storageHandler, IResourceCrawler resourceCrawler, IFactory drugCheckingSourceFactory)
        {
            _storageHandler = storageHandler;
            _resourceCrawler = resourceCrawler;
            _drugCheckingSourceFactory = drugCheckingSourceFactory;
        }

        public async Task UpdateResources()
        {
            var crawlingResult = _resourceCrawler.Crawl(1);

            await foreach (var item in crawlingResult.Items)
            {
                var entity = await _drugCheckingSourceFactory.Create(item);
                await _storageHandler.UpdateResources(entity);
            }
        }
    }
}
