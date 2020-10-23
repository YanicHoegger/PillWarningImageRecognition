using System.Collections.Generic;
using ImageInteraction.Interface;

namespace Domain
{
    public class TrainingImage : ITrainingImage
    {
        public TrainingImage(byte[] image, IEnumerable<string> tags)
        {
            Image = image;
            Tags = tags;
        }

        public byte[] Image { get; }
        public IEnumerable<string> Tags { get; }
    }
}
