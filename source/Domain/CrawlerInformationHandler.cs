using DatabaseInteraction;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
    public class CrawlerInformationHandler
    {
        private readonly Repository<CrawlerAction> _crawlerRepository;
        private readonly Repository<DrugCheckingSource> _drugCheckingRepository;

        private const string Number = nameof(Number);
        private static readonly string NumberRegex = @$"https:\/\/de\.drugchecking\.ch\/pdf\.php\?p=(?<{Number}>[0-9]+)";

        public CrawlerInformationHandler(IContext context)
        {
            _crawlerRepository = new Repository<CrawlerAction>(context);
            _drugCheckingRepository = new Repository<DrugCheckingSource>(context);
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
            await _crawlerRepository.Insert(new CrawlerAction
            {
                Executed = DateTime.Now,
                LastSuccessfullIndex = lastSuccessfullIndex,
                CrawlingCount = crawlingCount
            });
        }

        private static int GetIndex(DrugCheckingSource drugCheckingSource)
        {
            var numberMatch = Regex.Match(drugCheckingSource.PdfLocation, NumberRegex);
            var numberString = numberMatch.Groups[Number].Value;
            return int.Parse(numberString);
        }
    }
}
