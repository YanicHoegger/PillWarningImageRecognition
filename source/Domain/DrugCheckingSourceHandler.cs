using DatabaseInteraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class DrugCheckingSourceHandler
    {
        private readonly Repository<DrugCheckingSource> _repository;

        public DrugCheckingSourceHandler(IContext context)
        {
            _repository = new Repository<DrugCheckingSource>(context);
        }

        public async Task StoreSources(IEnumerable<DrugCheckingSource> sources)
        {
            var alreadyPresent = await _repository.Get();

            var notPresentSources = sources.Where(x => !CheckIfPresent(alreadyPresent, x));

            await _repository.Insert(notPresentSources);
        }

        public async Task StoreSources(DrugCheckingSource source)
        {
            var alreadyPresent = await _repository.Get();

            if(CheckIfPresent(alreadyPresent, source))
            {
                return;
            }

            await _repository.Insert(source);
        }

        private bool CheckIfPresent(IEnumerable<DrugCheckingSource> alreadyPresent, DrugCheckingSource toCheck)
        {
            return alreadyPresent.Any(y => y.DocumentHash.Equals(toCheck.DocumentHash));
        }
    }
}
