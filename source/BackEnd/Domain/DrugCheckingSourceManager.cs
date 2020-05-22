using CustomVisionInteraction.Interface;
using DatabaseInteraction.Interface;
using DrugCheckingCrawler.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class DrugCheckingSourceManager
    {
        private readonly CrawlerInformationHandler _crawlerInformationHandler;
        private readonly IColorAnalyzer _colorAnalyzer;
        private readonly DrugCheckingSourceHandler _drugCheckingSourceHandler;
        private readonly IPillRecognitionTrainer _trainer;
        private readonly IResourceCrawler _resourceCrawler;
        private readonly IEntityFactory _entityFactory;

        public DrugCheckingSourceManager(CrawlerInformationHandler crawlerInformationHandler, DrugCheckingSourceHandler drugCheckingSourceHandler, IColorAnalyzer colorAnalyzer, IPillRecognitionTrainer trainer, IResourceCrawler resourceCrawler, IEntityFactory entityFactory)
        {
            _crawlerInformationHandler = crawlerInformationHandler;
            _drugCheckingSourceHandler = drugCheckingSourceHandler;
            _trainer = trainer;
            _colorAnalyzer = colorAnalyzer;
            _resourceCrawler = resourceCrawler;
            _entityFactory = entityFactory;
        }

        public async Task SetUpResources()
        {
            var crawlingResult = await CrawlResources();

            if (!crawlingResult.Items.Any())
                return;

            var storeResources = StoreResources(crawlingResult);
            var train = TrainCustomVision(crawlingResult);

            await storeResources;
            await train;
        }

        private async Task<ICrawlerResult> CrawlResources()
        {
            var lastIndex = await _crawlerInformationHandler.GetLastIndex();
            var crawlingResult = await _resourceCrawler.Crawl(lastIndex + 1);

            return crawlingResult;
        }

        private async Task StoreResources(ICrawlerResult crawlerResult)
        {
            await StoreItems(crawlerResult);
            await StoreCrawlingResult(crawlerResult);
        }

        private async Task StoreItems(ICrawlerResult crawlerResult)
        {

            foreach (var item in crawlerResult.Items)
            {
                var entity = _entityFactory.Create<DrugCheckingSource>();

                var color = await _colorAnalyzer.GetColor(item.Image);

                entity.Name = item.Name;
                entity.Color = color;
                entity.Creation = item.Tested;
                entity.PdfLocation = item.Url;
                entity.Image = item.Image;
                entity.DocumentHash = item.DocumentHash;

                await _drugCheckingSourceHandler.StoreSources(entity);
            }
        }

        private async Task StoreCrawlingResult(ICrawlerResult crawlerResult)
        {
            await _crawlerInformationHandler.Insert(crawlerResult.LastSuccessfullIndex, crawlerResult.Items.Count());
        }

        private async Task TrainCustomVision(ICrawlerResult crawlerResult)
        {
            await _trainer.Train(crawlerResult.Items.Select(x => (x.Image, x.Name)));
        }
    }
}
