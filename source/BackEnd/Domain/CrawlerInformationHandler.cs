using DatabaseInteraction.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class CrawlerInformationHandler
    {
        private readonly IRepository<ICrawlerAction> _crawlerRepository;
        private readonly IEntityFactory _entityFactory;

        public CrawlerInformationHandler(IRepository<ICrawlerAction> crawlerRepository, IEntityFactory entityFactory)
        {
            _crawlerRepository = crawlerRepository;
            _entityFactory = entityFactory;
        }

        public async Task<int> GetLastIndex()
        {
            var entries = await _crawlerRepository.Get().ToListAsync();
            return entries.Any() ? entries.Max(x => x.LastSuccessfulIndex) : 0;
        }

        public async Task Insert(int lastSuccessfulIndex, int crawlingCount)
        {
            var toInsert = _entityFactory.Create<ICrawlerAction>();

            toInsert.LastSuccessfulIndex = lastSuccessfulIndex;
            toInsert.CrawlingCount = crawlingCount;
            toInsert.Executed = DateTime.Now;

            await _crawlerRepository.Insert(toInsert);
        }
    }
}
