using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomVisionInteraction
{
    public class TrainerCommunicator : ITrainerCommunicator
    {
        private readonly IContext _context;
        private readonly CustomVisionTrainingClient _customVisionTrainingClient;

        private IList<Tag> _tags;
        private IList<Image> _images;

        public TrainerCommunicator(IContext context)
        {
            _context = context;
            _customVisionTrainingClient = new CustomVisionTrainingClient
            {
                ApiKey = _context.Key,
                Endpoint = _context.EndPoint
            };
        }

        public bool IsInitialized { get; private set; }

        public async Task Init()
        {
            _tags = await _customVisionTrainingClient.GetTagsAsync(_context.ProjectId);
            _images = (await _customVisionTrainingClient.GetTaggedImagesAsync(_context.ProjectId))
                .Concat(await _customVisionTrainingClient.GetUntaggedImagesAsync(_context.ProjectId))
                .ToList();

            IsInitialized = true;
        }

        public async Task<Tag> CreateTag(string name)
        {
            ThrowIfNotInitialized();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(name)} can not be empty");

            if (_tags.Select(x => x.Name).Contains(name))
                throw new InvalidOperationException($"Tag with name {name} already exists");

            var tag = await _customVisionTrainingClient.CreateTagAsync(_context.ProjectId, name);
            _tags.Add(tag);

            return tag;
        }

        public Tag GetTag(string tagName)
        {
            ThrowIfNotInitialized();

            return _tags.SingleOrDefault(x => x.Name.Equals(tagName));
        }

        public async Task AddImage(Stream imageStream, IEnumerable<Tag> tags)
        {
            ThrowIfNotInitialized();

            await _customVisionTrainingClient.CreateImagesFromDataAsync(_context.ProjectId, imageStream, tags.Select(x => x.Id).ToList());
        }

        public IEnumerable<Task<byte[]>> DownloadImages()
        {
            ThrowIfNotInitialized();

            foreach (var image in _images)
            {
                yield return new WebClient().DownloadDataTaskAsync(image.OriginalImageUri);
            }
        }

        private void ThrowIfNotInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException($"Need to call {nameof(Init)} first");
        }
    }
}
