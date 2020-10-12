using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace ImageInteraction.UnitTests
{
    public class TrainerCommunicatorMock : ITrainerCommunicator
    {
        public bool IsInitialized { get; set; } = true;

        public IList<Tag> Tags { get; set; } = new List<Tag>();

        public List<(byte[] image, IEnumerable<Tag> tags)> AddedImage { get; } = new List<(byte[] image, IEnumerable<Tag> tags)>();

        public List<byte[]> DownloadingImages { get; set; } = new List<byte[]>();

        public Task AddImage(Stream imageStream, IEnumerable<Tag> tags)
        {
            using var memoryStream = new MemoryStream();

            imageStream.CopyTo(memoryStream);
            AddedImage.Add((memoryStream.ToArray(), tags));

            return Task.CompletedTask;
        }

        public Task<Tag> CreateTag(string name)
        {
            return Task.FromResult(new Tag { Name = name });
        }

        public IEnumerable<Task<byte[]>> DownloadImages()
        {
            return DownloadingImages.Select(Task.FromResult);
        }

        public Tag GetTag(string tagName)
        {
            return Tags.SingleOrDefault(x => x.Name.Equals(tagName));
        }
    }
}
