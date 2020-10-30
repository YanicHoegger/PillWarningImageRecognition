using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace ImageInteraction.Training
{
    public class ClassificationTrainer : IClassificationTrainer
    {
        private readonly ITrainerCommunicator _trainerCommunicator;

        private const int _minimumTagCount = 5;

        public ClassificationTrainer(ITrainerCommunicator trainerCommunicator)
        {
            _trainerCommunicator = trainerCommunicator;
        }

        public async Task Train(IEnumerable<ITrainingImage> trainingImages)
        {
            var trainingImagesList = trainingImages.ToList();

            var tags = trainingImagesList
                .SelectMany(x => x.Tags)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var trainingImage in trainingImagesList)
            {
                if (trainingImage.Tags.All(x =>
                {
                    var existingTagCount = _trainerCommunicator.GetTag(x)?.ImageCount ?? 0;

                    return existingTagCount + tags[x] >= _minimumTagCount;
                }))
                {
                    await AddImageToTrain(new MemoryStream(trainingImage.Image), trainingImage.Tags);
                }
            }
        }

        private async Task AddImageToTrain(Stream imageStream, IEnumerable<string> tag)
        {
            var tags = await Task.WhenAll(tag.Select(GetOrCreateTag));

            await _trainerCommunicator.AddImage(imageStream, tags);
        }

        private async Task<Tag> GetOrCreateTag(string tagName)
        {
            var tag = _trainerCommunicator.GetTag(tagName);

            if (tag != null)
                return tag;

            return await _trainerCommunicator.CreateTag(tagName);
        }
    }
}
