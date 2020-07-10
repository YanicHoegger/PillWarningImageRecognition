using DatabaseInteraction.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class DrugCheckingSourceHandler
    {
        private readonly IRepository<DrugCheckingSource> _repository;
        private readonly IDataBaseUpdater _dataBaseUpdater;

        public DrugCheckingSourceHandler(IRepositoryFactory factory, IDataBaseUpdater dataBaseUpdater)
        {
            _repository = factory.Create<DrugCheckingSource>();
            _dataBaseUpdater = dataBaseUpdater;
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

            if (CheckIfPresent(alreadyPresent, source))
            {
                return;
            }

            await _repository.Insert(source);
        }

        public async Task UpdateResources(DrugCheckingSource toUpdate)
        {
            await _dataBaseUpdater.Update(_repository, toUpdate, x => CompareFunction(x, toUpdate));
        }

        private bool CheckIfPresent(IEnumerable<DrugCheckingSource> alreadyPresent, DrugCheckingSource toCheck)
        {
            return alreadyPresent.Any(y => CompareFunction(y, toCheck));
        }

        private static bool CompareFunction(DrugCheckingSource x, DrugCheckingSource y)
        {
            return x.PdfLocation.Equals(y.PdfLocation);
        }
    }
}
