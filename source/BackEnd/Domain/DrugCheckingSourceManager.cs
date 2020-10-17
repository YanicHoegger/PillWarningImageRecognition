using System.Collections.Generic;
using DrugCheckingCrawler.Interface;
using System.Linq;
using System.Threading.Tasks;
using Domain.Prediction;
using ImageInteraction.Interface;

namespace Domain
{
    public class DrugCheckingSourceManager
    {
        private readonly CrawlerInformationHandler _crawlerInformationHandler;
        private readonly DrugCheckingSourceHandler _drugCheckingSourceHandler;
        private readonly IClassificationTrainer _trainer;
        private readonly IResourceCrawler _resourceCrawler;
        private readonly IPillRecognizer _pillRecognizer;
        private readonly IDrugCheckingSourceFactory _drugCheckingSourceFactory;

        public DrugCheckingSourceManager(CrawlerInformationHandler crawlerInformationHandler,
            DrugCheckingSourceHandler drugCheckingSourceHandler,
            IClassificationTrainer trainer,
            IResourceCrawler resourceCrawler,
            IPillRecognizer pillRecognizer,
            IDrugCheckingSourceFactory drugCheckingSourceFactory)
        {
            _crawlerInformationHandler = crawlerInformationHandler;
            _drugCheckingSourceHandler = drugCheckingSourceHandler;
            _trainer = trainer;
            _resourceCrawler = resourceCrawler;
            _pillRecognizer = pillRecognizer;
            _drugCheckingSourceFactory = drugCheckingSourceFactory;
        }

        public async Task SetUpResources()
        {
            var crawlingResult = await CrawlResources();

            var filtered = FilterNoPills(crawlingResult.Items);

            var storeResources = await StoreAndReturnResources(filtered, crawlingResult);

            await TrainCustomVision(storeResources);
        }

        public async Task UpdateResources()
        {
            var crawlingResult = _resourceCrawler.Crawl(1);

            await foreach (var item in crawlingResult.Items)
            {
                var entity = await _drugCheckingSourceFactory.Create(item);
                await _drugCheckingSourceHandler.UpdateResources(entity);
            }
        }

        private async Task<ICrawlerResult> CrawlResources()
        {
            var lastIndex = await _crawlerInformationHandler.GetLastIndex();
            var crawlingResult = _resourceCrawler.Crawl(lastIndex + 1);

            return crawlingResult;
        }

        private async IAsyncEnumerable<ICrawlerResultItem> FilterNoPills(IAsyncEnumerable<ICrawlerResultItem> toFilter)
        {
            await foreach (var crawlerResultItem in toFilter)
            {
                if (await _pillRecognizer.IsPill(crawlerResultItem.Image))
                    yield return crawlerResultItem;
            }
        }

        private async Task<IList<ICrawlerResultItem>> StoreAndReturnResources(IAsyncEnumerable<ICrawlerResultItem> items, ICrawlerResult crawlerResult)
        {
            var resultList = new List<ICrawlerResultItem>();

            await foreach (var crawlerResultItem in items)
            {
                resultList.Add(crawlerResultItem);
                await StoreItem(crawlerResultItem);
            }

            await StoreCrawlingResult(await crawlerResult.LastSuccessfulIndex, resultList.Count);

            return resultList;
        }

        private async Task StoreItem(ICrawlerResultItem item)
        {
            var entity = await _drugCheckingSourceFactory.Create(item);
            await _drugCheckingSourceHandler.StoreSources(entity);
        }

        private async Task StoreCrawlingResult(int index, int count)
        {
            await _crawlerInformationHandler.Insert(index, count);
        }

        private async Task TrainCustomVision(IEnumerable<ICrawlerResultItem> items)
        {
            await _trainer.Train(items.Select(x => (x.Image, x.Name)));
        }
    }
}
