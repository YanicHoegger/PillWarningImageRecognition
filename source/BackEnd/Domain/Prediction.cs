using DatabaseInteraction.Interface;
using Domain.Interface;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;
using IPredictionResult = Domain.Interface.IPredictionResult;

namespace Domain
{
    public class Prediction : IPredicition
    {
        private const string _pillTag = "Pill";
        private const double _minimumProbabilityPill = 0.8;
        private const double _minimumProbabilityTag = 0.2;

        private readonly IRepository<DrugCheckingSource> _repository;
        private readonly IClassifier _classifier;
        private readonly IPillColorAnalyzer _pillColorAnalyzer;

        public Prediction(IRepositoryFactory repositoryFactory, IClassifier classifier, IPillColorAnalyzer pillColorAnalyzer)
        {
            //TODO: Move factory call to DI
            _repository = repositoryFactory.Create<DrugCheckingSource>();
            _classifier = classifier;
            _pillColorAnalyzer = pillColorAnalyzer;
        }

        public async Task<IPredictionResult> Predict(byte[] image)
        {
            var classificationResult = await _classifier.GetImageClassification(image);

            if (!IsPill(classificationResult))
                return null;

            var color = await GetColor(image);

            var tagFindings = await GetTagFindings(FilterClassification(classificationResult).Select(x => x.TagName).ToList(), color);
            var colorFindings = (await _repository.Get()).Where(x => x.Color.Equals(color)).OrderByDescending(x => x.Creation).Take(20);

            return new PredictionResult(tagFindings, colorFindings);
        }

        private static bool IsPill(IEnumerable<IClassificationResult> classificationResult)
        {
            var pillClassification = classificationResult.SingleOrDefault(x => x.TagName.Equals(_pillTag));

            return pillClassification != null && pillClassification.Probability > _minimumProbabilityPill;
        }

        //TODO: have different strategies: Maybe analyze what the best reult is and depending on that decide what other results should be displayed
        private static IEnumerable<IClassificationResult> FilterClassification(IEnumerable<IClassificationResult> toFileter)
        {
            return toFileter.Where(x => !x.TagName.Equals(_pillTag) && x.Probability > _minimumProbabilityTag);
        }

        private async Task<Color> GetColor(byte[] image)
        {
            return await _pillColorAnalyzer.GetColor(image);
        }

        private async Task<List<DrugCheckingSource>> GetTagFindings(IList<string> tags, Color color)
        {
            var tagFindings = new List<DrugCheckingSource>();
            foreach (var tag in tags)
            {
                tagFindings.AddRange((await _repository.Get()).Where(x => x.Name.Equals(tag)));
            }

            var sortedList = new List<DrugCheckingSource>();
            var i = 0;
            for (; i < tags.Count(); i++)
            {
                sortedList.AddRange(tagFindings.Where(x => x.Name.Equals(tags.ElementAt(i)) && x.Color.Equals(color)).OrderByDescending(x => x.Creation));

                if (i > 0)
                {
                    sortedList.AddRange(tagFindings.Where(x => x.Name.Equals(tags.ElementAt(i - 1)) && !x.Color.Equals(color)).OrderByDescending(x => x.Creation));
                }
            }
            sortedList.AddRange(tagFindings.Where(x => x.Name.Equals(tags.ElementAt(i - 1)) && !x.Color.Equals(color)).OrderByDescending(x => x.Creation));

            return sortedList;
        }
    }
}
