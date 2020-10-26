using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Utilities;

namespace ImageInteraction.Training
{
    public class ClassificationTrainer : IClassificationTrainer
    {
        private readonly ITrainerCommunicator _trainerCommunicator;

        private const int _minimumTagCount = 5;

        public ClassificationTrainer(ITrainerCommunicator trainerCommunicator)
        {
            if (!trainerCommunicator.IsInitialized)
                throw new ArgumentException($"{nameof(trainerCommunicator)} has to be initialized");

            _trainerCommunicator = trainerCommunicator;
        }

        public async Task Train(IEnumerable<ITrainingImage> trainingImages)
        {
            var filtered = (await FilterInput(trainingImages)).ToList();

            var tags = filtered
                .SelectMany(x => x.Tags)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var trainingImage in filtered)
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

        //TODO: Check if this is not already managed by custom vision
        /// <summary>
        /// To filter images that already are present in training
        /// </summary>
        private async Task<IEnumerable<ITrainingImage>> FilterInput(IEnumerable<ITrainingImage> trainingImages)
        {
            //Make hash set for quicker comparision
            var hashTable = new HashSet<byte[]>(new ByteArrayComparer());
            await foreach (var task in _trainerCommunicator.DownloadImages())
            {
                hashTable.Add(task);
            }

            return trainingImages.Where(x =>
            {
                if (hashTable.Contains(x.Image))
                    return false;

                hashTable.Add(x.Image);
                return true;
            });
        }
    }
}
