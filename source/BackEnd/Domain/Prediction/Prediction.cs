using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Domain.Interface;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public class Prediction : IPredicition
    {
        private const string _pillTag = "Pill";

        private readonly IRepository<DrugCheckingSource> _repository;
        private readonly IClassifier _classifier;
        private readonly IPillColorAnalyzer _pillColorAnalyzer;
        private readonly IProbabilityToLikelinessConverter _converter;

        public Prediction(IRepositoryFactory repositoryFactory, IClassifier classifier, IPillColorAnalyzer pillColorAnalyzer, IProbabilityToLikelinessConverter converter)
        {
            //TODO: Move factory call to DI
            _repository = repositoryFactory.Create<DrugCheckingSource>();
            _classifier = classifier;
            _pillColorAnalyzer = pillColorAnalyzer;
            _converter = converter;
        }

        public async Task<IPredictionResult> Predict(byte[] image)
        {
            var classificationResult = (await _classifier.GetImageClassification(image)).ToList();

            var pillLikeliness = GetPillLikeliness(classificationResult);

            //The likeliness the image is a pill is to small
            if (pillLikeliness < Likeliness.Maybe)
                return new PredictionResult(pillLikeliness, Enumerable.Empty<IFinding>(), Enumerable.Empty<IPillWarning>());

            var color = await GetColor(image);

            var tagFindings = await GetTagFindings(RemovePillTagClassification(classificationResult), color);
            var colorFindings = await ColorFindings(color);

            return new PredictionResult(pillLikeliness, tagFindings, colorFindings);
        }

        private async Task<IEnumerable<IPillWarning>> ColorFindings(Color color)
        {
            return (await _repository.Get())
                .Where(x => x.Color.Equals(color))
                .OrderByDescending(x => x.Creation)
                .Take(20)
                .Select(Convert);
        }

        private Likeliness GetPillLikeliness(IEnumerable<IClassificationResult> classificationResult)
        {
            var pillClassification = classificationResult.SingleOrDefault(x => x.TagName.Equals(_pillTag));

            return pillClassification == null 
                ? Likeliness.NotAtAll 
                : _converter.Convert(pillClassification.Probability);
        }

        private static IEnumerable<IClassificationResult> RemovePillTagClassification(IEnumerable<IClassificationResult> toFilter)
        {
            return toFilter.Where(x => !x.TagName.Equals(_pillTag));
        }

        private async Task<Color> GetColor(byte[] image)
        {
            return await _pillColorAnalyzer.GetColor(image);
        }

        private async Task<List<Finding>> GetTagFindings(IEnumerable<IClassificationResult> classificationResults, Color color)
        {
            var findings = new List<Finding>();
            var resources = await _repository.Get();

            // ReSharper disable once LoopCanBeConvertedToQuery : Is better readable
            foreach (var classificationResult in classificationResults)
            {
                var likeliness = _converter.Convert(classificationResult.Probability);

                if(likeliness < Likeliness.Maybe)
                    continue;

                var correlatedTagResources = resources.Where(x => x.Name.Equals(classificationResult.TagName));

                var orderedResources = correlatedTagResources
                    .OrderByDescending(x => x.Color.Equals(color) )
                    .ThenByDescending(x => x.Creation)
                    .Select(Convert);

                findings.Add(new Finding(classificationResult.TagName, likeliness, orderedResources));
            }

            return findings;
        }

        private static IPillWarning Convert(DrugCheckingSource drugCheckingSource)
        {
            return new PillWarning(drugCheckingSource);
        }
    }
}
