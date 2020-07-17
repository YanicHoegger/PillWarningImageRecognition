using CustomVisionInteraction.Interface;
using DatabaseInteraction.Interface;
using Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPredictionResult = Domain.Interface.IPredictionResult;

namespace Domain
{
    public class Predicition : IPredicition
    {
        private readonly IRepository<DrugCheckingSource> _repository;
        private readonly IPrediction _prediction;

        public Predicition(IRepositoryFactory repositoryFactory, IPrediction prediction)
        {
            _repository = repositoryFactory.Create<DrugCheckingSource>();
            _prediction = prediction;
        }

        public async Task<IPredictionResult> Predict(byte[] image)
        {
            var internalResult = await _prediction.PredictImage(image);

            if (!internalResult.Success)
                return null;

            var tagFindings = await GetTagFindings(internalResult);
            var colorFindings = (await _repository.Get()).Where(x => x.Color.Equals(internalResult.Color)).OrderByDescending(x => x.Creation).Take(20);

            return new PredictionResult(tagFindings, colorFindings);
        }

        private async Task<List<DrugCheckingSource>> GetTagFindings(CustomVisionInteraction.Interface.IPredictionResult internalResult)
        {
            var tagFindings = new List<DrugCheckingSource>();
            foreach (var tag in internalResult.Tags)
            {
                tagFindings.AddRange((await _repository.Get()).Where(x => x.Name.Equals(tag)));
            }

            var sortedList = new List<DrugCheckingSource>();
            var i = 0;
            for (; i < internalResult.Tags.Count(); i++)
            {
                sortedList.AddRange(tagFindings.Where(x => x.Name.Equals(internalResult.Tags.ElementAt(i)) && x.Color.Equals(internalResult.Color)).OrderByDescending(x => x.Creation));

                if (i > 0)
                {
                    sortedList.AddRange(tagFindings.Where(x => x.Name.Equals(internalResult.Tags.ElementAt(i - 1)) && !x.Color.Equals(internalResult.Color)).OrderByDescending(x => x.Creation));
                }
            }
            sortedList.AddRange(tagFindings.Where(x => x.Name.Equals(internalResult.Tags.ElementAt(i - 1)) && !x.Color.Equals(internalResult.Color)).OrderByDescending(x => x.Creation));

            return sortedList;
        }
    }
}
