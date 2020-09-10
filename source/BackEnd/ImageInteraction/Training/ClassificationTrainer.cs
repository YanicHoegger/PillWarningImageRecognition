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

        //TODO: _pillTag should be added in domain
        private const string _pillTag = "Pill";
        private const int _minimumTagCount = 5;

        public ClassificationTrainer(ITrainerCommunicator trainerCommunicator)
        {
            if (!trainerCommunicator.IsInitialized)
                throw new ArgumentException($"{nameof(trainerCommunicator)} has to be initialized");

            _trainerCommunicator = trainerCommunicator;
        }

        public async Task Train(IEnumerable<(byte[] image, string tag)> inputData)
        {
            var filtered = await FilterInput(inputData);

            foreach (var group in filtered.GroupBy(x => x.tag))
            {
                var existingTagCount = _trainerCommunicator.GetTag(group.Key)?.ImageCount ?? 0;

                if (existingTagCount + group.Count() < _minimumTagCount)
                    continue;

                foreach (var (image, tag) in group)
                {
                    await AddImageToTrain(new MemoryStream(image), tag);
                }
            }
        }

        private async Task AddImageToTrain(Stream imageStream, string tag)
        {
            var pillTag = _trainerCommunicator.GetTag(_pillTag);
            var specificTag = await GetOrCreateTag(tag);

            await _trainerCommunicator.AddImage(imageStream, new[] { pillTag, specificTag });
        }

        private async Task<Tag> GetOrCreateTag(string tagName)
        {
            var tag = _trainerCommunicator.GetTag(tagName);

            if (tag != null)
                return tag;

            return await _trainerCommunicator.CreateTag(tagName);
        }

        /// <summary>
        /// To filter images that already are present in training
        /// </summary>
        private async Task<IEnumerable<(byte[] image, string tag)>> FilterInput(IEnumerable<(byte[] image, string tag)> inputData)
        {
            //Make hash set for quicker comparision
            var hashTable = new HashSet<byte[]>(new ByteArrayComparer());
            foreach (var task in _trainerCommunicator.DownloadImages())
            {
                hashTable.Add(await task);
            }

            return inputData.Where(x =>
            {
                if (hashTable.Contains(x.image))
                    return false;

                hashTable.Add(x.image);
                return true;
            });
        }
    }
}
