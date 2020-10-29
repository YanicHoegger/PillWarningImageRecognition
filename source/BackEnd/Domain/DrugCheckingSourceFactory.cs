using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using DrugCheckingCrawler.Interface;
using ImageInteraction.Interface;

namespace Domain
{
    public class DrugCheckingSourceFactory : IDrugCheckingSourceFactory
    {
        private readonly IColorAnalyzer _colorAnalyzer;
        private readonly IEntityFactory _entityFactory;

        public DrugCheckingSourceFactory(IColorAnalyzer colorAnalyzer, IEntityFactory entityFactory)
        {
            _colorAnalyzer = colorAnalyzer;
            _entityFactory = entityFactory;
        }

        public async Task<IDrugCheckingSource> Create(ICrawlerResultItem item)
        {
            var entity = _entityFactory.Create<IDrugCheckingSource>();

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
                .Select(x => CreateDrugCheckingInfo(x.Title, x.Info))
                .ToList();

            entity.SaferUseRulesTitle = item.SaferUserRules.Title;
            entity.SaferUseRules = item.SaferUserRules.Rules.ToList();

            return entity;
        }

        private IDrugCheckingInfo CreateDrugCheckingInfo(string title, string info)
        {
            var drugCheckingInfo = _entityFactory.Create<IDrugCheckingInfo>();

            drugCheckingInfo.Title = title;
            drugCheckingInfo.Info = info;

            return drugCheckingInfo;
        }
    }
}
