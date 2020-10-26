using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Hosting;

namespace ImageInteraction.Training
{
    public class TrainerCommunicator : ITrainerCommunicator, IHostedService
    {
        private const int _maxImagesPreRequest = 256;

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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _tags = await _customVisionTrainingClient.GetTagsAsync(_context.ProjectId, cancellationToken: cancellationToken);
            _images = (await GetTaggedImages(cancellationToken).ToListAsync(cancellationToken))
                .Concat(await GetUntaggedImages(cancellationToken).ToListAsync(cancellationToken))
                .ToList();

            IsInitialized = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public bool IsInitialized { get; private set; }

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

        public async IAsyncEnumerable<byte[]> DownloadImages()
        {
            ThrowIfNotInitialized();

            foreach (var image in _images)
            {
                yield return await new WebClient().DownloadDataTaskAsync(image.OriginalImageUri);
            }
        }

        private void ThrowIfNotInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Must be initialized first");
        }

        private IAsyncEnumerable<Image> GetTaggedImages(CancellationToken cancellationToken)
        {
            return GetImages(skip => _customVisionTrainingClient.GetTaggedImagesAsync(_context.ProjectId, 
                cancellationToken: cancellationToken, 
                take: _maxImagesPreRequest, 
                skip: skip));
        }

        private IAsyncEnumerable<Image> GetUntaggedImages(CancellationToken cancellationToken)
        {
            return GetImages(skip => _customVisionTrainingClient.GetUntaggedImagesAsync(_context.ProjectId,
                cancellationToken: cancellationToken,
                take: _maxImagesPreRequest,
                skip: skip));
        }

        private static async IAsyncEnumerable<Image> GetImages(Func<int, Task<IList<Image>>> getterFunc)
        {
            IList<Image> results;
            int skip = 0;
            do
            {
                results = await getterFunc(skip);

                skip += _maxImagesPreRequest;

                foreach (var image in results)
                {
                    yield return image;
                }
            } while (results.Count >= _maxImagesPreRequest);
        }
    }
}
