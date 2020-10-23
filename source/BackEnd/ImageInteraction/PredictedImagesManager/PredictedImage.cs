using System.Collections.Generic;
using ImageInteraction.Classification;
using ImageInteraction.Interface;

namespace ImageInteraction.PredictedImagesManager
{
    public class PredictedImage : ImageClassificationResult, IPredictedImage
    {
        public PredictedImage(IEnumerable<ITagClassificationResult> tagClassifications, byte[] image, string id) 
            : base(tagClassifications)
        {
            Image = image;
            Id = id;
        }

        public byte[] Image { get; }

        public string Id { get; }
    }
}
