using CustomVisionInteraction.Interface;
using DatabaseInteraction.Interface;
using DrugCheckingCrawler.Interface;
using System.Data;
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

        public DrugCheckingSourceManager(CrawlerInformationHandler crawlerInformationHandler, 
            DrugCheckingSourceHandler drugCheckingSourceHandler, 
            IColorAnalyzer colorAnalyzer, 
            IPillRecognitionTrainer trainer, 
            IResourceCrawler resourceCrawler, 
            IEntityFactory entityFactory)
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

        public async Task UpdateResources()
        {
            var crawlingResult = await _resourceCrawler.Crawl(1);

            foreach(var item in crawlingResult.Items)
            {
                var entity = await CreateEntity(item);
                await _drugCheckingSourceHandler.UpdateResources(entity);
            }
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
                var entity = await CreateEntity(item);
                await _drugCheckingSourceHandler.StoreSources(entity);
            }
        }

        private async Task<DrugCheckingSource> CreateEntity(ICrawlerResultItem item)
        {
            var entity = _entityFactory.Create<DrugCheckingSource>();

            var color = await _colorAnalyzer.GetColor(item.Image);

            entity.Header = item.Header;
            entity.Name = item.Name;
            entity.Color = color;
            entity.Creation = item.Tested;
            entity.PdfLocation = item.Url;
            entity.Image = item.Image;
            entity.DocumentHash = item.DocumentHash;
            entity.GeneralInfos = item.GeneralInfos;

            entity.RiskEstimationTitle = item.RiskEstimation.Title;
            entity.RiskEstimation = item.RiskEstimation.RiskEstimation;

            entity.Infos = item
                .Infos
                .Select(x => new DrugCheckingInfo { Title = x.Title, Info = x.Info })
                .ToList();

            entity.SaferUseRulesTitle = item.SaferUserRules.Title;
            entity.SaferUseRules = item.SaferUserRules.Rules.ToList();

            return entity;
        }

        private async Task StoreCrawlingResult(ICrawlerResult crawlerResult)
        {
            await _crawlerInformationHandler.Insert(crawlerResult.LastSuccessfullIndex, crawlerResult.Items.Count());
        }

        private async Task TrainCustomVision(ICrawlerResult crawlerResult)
        {
            //TODO: Check if images are pill
            await _trainer.Train(crawlerResult.Items.Select(x => (x.Image, x.Name)));
        }
    }
}
