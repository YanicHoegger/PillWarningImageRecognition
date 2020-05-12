using CustomVisionInteraction.Prediction;
using CustomVisionInteraction.Training;
using DatabaseInteraction;
using DrugCheckingCrawler;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class DrugCheckingSourceManager
    {
        private readonly CrawlerInformationHandler _crawlerInformationHandler;
        private readonly ColorAnalyzer _colorAnalyzer;
        private readonly DrugCheckingSourceHandler _drugCheckingSourceHandler;
        private readonly CustomVisionInteraction.IContext _trainingContext;

        public DrugCheckingSourceManager(IContext databaseContext, CustomVisionInteraction.IContext trainingContext, CustomVisionInteraction.IContext computerVisionContext, IPredictionContext detectionContext)
        {
            _crawlerInformationHandler = new CrawlerInformationHandler(databaseContext);
            _drugCheckingSourceHandler = new DrugCheckingSourceHandler(databaseContext);
            _colorAnalyzer = new ColorAnalyzer(new ComputerVisionCommunication(computerVisionContext), new PillDetection(new PillDetectionCommunication(detectionContext)));
            _trainingContext = trainingContext;
        }

        public async Task SetUpResources()
        {
            CrawlerResult crawlingResult = await CrawlResources();

            if (!crawlingResult.Items.Any())
                return;

            var storeResources = StoreResources(crawlingResult);
            var train = TrainCustomVision(crawlingResult);

            await storeResources;
            await train;
        }

        private async Task<CrawlerResult> CrawlResources()
        {
            var lastIndex = await _crawlerInformationHandler.GetLastIndex();

            var resourceCrawler = new ResourceCrawler();
            var crawlingResult = await resourceCrawler.Crawl(lastIndex + 1);

            return crawlingResult;
        }

        private async Task StoreResources(CrawlerResult crawlerResult)
        {
            await StoreItems(crawlerResult);
            await StoreCrawlingResult(crawlerResult);
        }

        private async Task StoreItems(CrawlerResult crawlerResult)
        {
            foreach (var item in crawlerResult.Items)
            {
                var color = await _colorAnalyzer.GetColor(item.ParserResult.Image);

                await _drugCheckingSourceHandler.StoreSources(new DrugCheckingSource
                {
                    Name = item.ParserResult.Name,
                    Color = color,
                    Creation = item.ParserResult.Tested,
                    PdfLocation = item.Url,
                    Image = item.ParserResult.Image,
                    DocumentHash = item.DocumentHash
                });
            }
        }

        private async Task StoreCrawlingResult(CrawlerResult crawlerResult)
        {
            await _crawlerInformationHandler.Insert(crawlerResult.LastSuccessfullIndex, crawlerResult.Items.Count);
        }

        private async Task TrainCustomVision(CrawlerResult crawlerResult)
        {
            var trainer = await PillRecognitionTrainerFactory.Create(_trainingContext);

            await trainer.Train(crawlerResult.Items.Select(x => (x.ParserResult.Image, x.ParserResult.Name)));
        }
    }
}
