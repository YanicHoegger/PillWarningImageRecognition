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
        private readonly IRepository<DrugCheckingSource> _repository;
        private readonly IClassifier _classifier;
        private readonly IPillColorAnalyzer _pillColorAnalyzer;
        private readonly IProbabilityToLikelinessConverter _converter;
        private readonly IPillRecognizer _pillRecognizer;

        public Prediction(IRepository<DrugCheckingSource> repository, IClassifier classifier, IPillColorAnalyzer pillColorAnalyzer, IProbabilityToLikelinessConverter converter, IPillRecognizer pillRecognizer)
        {
            _repository = repository;
            _classifier = classifier;
            _pillColorAnalyzer = pillColorAnalyzer;
            _converter = converter;
            _pillRecognizer = pillRecognizer;
        }

        public async Task<IPredictionResult> Predict(byte[] image)
        {
            var classificationResult = (await _classifier.GetImageClassification(image)).ToList();

            if (!_pillRecognizer.IsPill(classificationResult))
                return PredictionResult.NoPillResult();

            var color = await GetColor(image);

            var tagFindings = await GetTagFindings(RemovePillTagClassification(classificationResult), color);
            var colorFindings = await ColorFindings(color);

            return PredictionResult.FromSuccess(tagFindings, colorFindings);
        }

        private async Task<IEnumerable<IPillWarning>> ColorFindings(Color color)
        {
            return (await _repository.Get())
                .Where(x => x.Color.Equals(color))
                .OrderByDescending(x => x.Creation)
                .Take(20)
                .Select(Convert);
        }

        private static IEnumerable<IClassificationResult> RemovePillTagClassification(IEnumerable<IClassificationResult> toFilter)
        {
            return toFilter.Where(x => !x.TagName.Equals(Constants.PillTag));
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
