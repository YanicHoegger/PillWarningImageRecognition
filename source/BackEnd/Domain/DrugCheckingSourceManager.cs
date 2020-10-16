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

            if (!crawlingResult.Items.Any())
                return;

            var filtered = await FilterNoPills(crawlingResult.Items);

            var storeResources = StoreResources(filtered, crawlingResult.LastSuccessfulIndex);
            var train = TrainCustomVision(filtered);

            await storeResources;
            await train;
        }

        public async Task UpdateResources()
        {
            var crawlingResult = await _resourceCrawler.Crawl(1);

            foreach(var item in crawlingResult.Items)
            {
                var entity = await _drugCheckingSourceFactory.Create(item);
                await _drugCheckingSourceHandler.UpdateResources(entity);
            }
        }

        private async Task<ICrawlerResult> CrawlResources()
        {
            var lastIndex = await _crawlerInformationHandler.GetLastIndex();
            var crawlingResult = await _resourceCrawler.Crawl(lastIndex + 1);

            return crawlingResult;
        }

        private async Task<IList<ICrawlerResultItem>> FilterNoPills(IEnumerable<ICrawlerResultItem> toFilter)
        {
            var resultList = new List<ICrawlerResultItem>();

            foreach (var crawlerResultItem in toFilter)
            {
                if (await _pillRecognizer.IsPill(crawlerResultItem.Image))
                    resultList.Add(crawlerResultItem);
            }

            return resultList;
        }

        private async Task StoreResources(IList<ICrawlerResultItem> items, int index)
        {
            await StoreItems(items);
            await StoreCrawlingResult(index, items.Count());
        }

        private async Task StoreItems(IEnumerable<ICrawlerResultItem> items)
        {
            foreach (var item in items)
            {
                var entity = await _drugCheckingSourceFactory.Create(item);
                await _drugCheckingSourceHandler.StoreSources(entity);
            }
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
