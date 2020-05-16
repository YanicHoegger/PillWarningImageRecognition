using DatabaseInteraction;
using DatabaseInteraction.Interface;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
    public class CrawlerInformationHandler
    {
        private readonly IRepository<CrawlerAction> _crawlerRepository;
        private readonly IRepository<DrugCheckingSource> _drugCheckingRepository;
        private readonly IEntityFactory _entityFactory;

        private const string Number = nameof(Number);
        private static readonly string NumberRegex = @$"https:\/\/de\.drugchecking\.ch\/pdf\.php\?p=(?<{Number}>[0-9]+)";

        public CrawlerInformationHandler(IRepositoryFactory repositoryFactory, IEntityFactory entityFactory)
        {
            _crawlerRepository = repositoryFactory.Create<CrawlerAction>();
            _drugCheckingRepository = repositoryFactory.Create<DrugCheckingSource>();
            _entityFactory = entityFactory;
        }

        public async Task<int> GetLastIndex()
        {
            //var entries = await _crawlerRepository.Get();
            //return entries.Any() ? entries.Max(x => x.LastSuccessfullIndex) : 0;

            var entries = await _drugCheckingRepository.Get();
            return entries.Any() ? entries.Max(GetIndex) : 0;
        }

        public async Task Insert(int lastSuccessfullIndex, int crawlingCount)
        {
            var toInsert = _entityFactory.Create<CrawlerAction>();

            toInsert.LastSuccessfullIndex = lastSuccessfullIndex;
            toInsert.CrawlingCount = crawlingCount;
            toInsert.Executed = DateTime.Now;

            await _crawlerRepository.Insert(toInsert);
        }

        private static int GetIndex(DrugCheckingSource drugCheckingSource)
        {
            var numberMatch = Regex.Match(drugCheckingSource.PdfLocation, NumberRegex);
            var numberString = numberMatch.Groups[Number].Value;
            return int.Parse(numberString);
        }
    }
}
