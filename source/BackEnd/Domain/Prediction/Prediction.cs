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
        private readonly IDrugCheckingSourceRepository _repository;
        private readonly IClassifier _classifier;
        private readonly IPillColorAnalyzer _pillColorAnalyzer;
        private readonly IProbabilityToLikelinessConverter _converter;
        private readonly IClassificationPillRecognizer _pillRecognizer;

        public Prediction(IDrugCheckingSourceRepository repository,
            IClassifier classifier,
            IPillColorAnalyzer pillColorAnalyzer,
            IProbabilityToLikelinessConverter converter,
            IClassificationPillRecognizer classificationPillRecognizer)
        {
            _repository = repository;
            _classifier = classifier;
            _pillColorAnalyzer = pillColorAnalyzer;
            _converter = converter;
            _pillRecognizer = classificationPillRecognizer;
        }

        public async Task<IPredictionResult> Predict(byte[] image)
        {
            var classificationResult = await _classifier.GetImageClassification(image);

            if (!_pillRecognizer.IsPill(classificationResult.TagClassifications))
                return PredictionResult.NoPillResult();

            var color = await GetColor(image);

            var tagFindings = GetTagFindings(RemovePillTagClassification(classificationResult.TagClassifications), color);
            var colorFindings = ColorFindings(color);

            return PredictionResult.FromSuccess(await tagFindings.ToListAsync(), await colorFindings.ToListAsync());
        }

        private IAsyncEnumerable<IPillWarning> ColorFindings(Color color)
        {
            return _repository.GetSameColor(color, 20).Select(Convert);
        }

        private static IEnumerable<ITagClassificationResult> RemovePillTagClassification(IEnumerable<ITagClassificationResult> toFilter)
        {
            return toFilter.Where(x => !x.TagName.Equals(Constants.PillTag));
        }

        private async Task<Color> GetColor(byte[] image)
        {
            return await _pillColorAnalyzer.GetColor(image);
        }

        private async IAsyncEnumerable<Finding> GetTagFindings(IEnumerable<ITagClassificationResult> classificationResults, Color color)
        {
            foreach (var classificationResult in classificationResults)
            {
                var likeliness = _converter.Convert(classificationResult.Probability);

                if (likeliness < Likeliness.Maybe)
                    continue;

                var orderedResources = await _repository.GetSameTagName(classificationResult.TagName)
                    .Select(Convert)
                    .OrderByDescending(x => x.Creation)
                    .ThenByDescending(x => x.Color.Equals(color))
                    .ToListAsync();

                yield return new Finding(classificationResult.TagName, likeliness, orderedResources);
            }
        }

        private static IPillWarning Convert(DrugCheckingSource drugCheckingSource)
        {
            return new PillWarning(drugCheckingSource);
        }
    }
}
