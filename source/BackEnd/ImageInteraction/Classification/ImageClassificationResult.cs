using System.Collections.Generic;
using ImageInteraction.Interface;

namespace ImageInteraction.Classification
{
    public class ImageClassificationResult : IImageClassificationResult
    {
        public ImageClassificationResult(IEnumerable<ITagClassificationResult> tagClassifications)
        {
            TagClassifications = tagClassifications;
        }

        public IEnumerable<ITagClassificationResult> TagClassifications { get; }
    }
}
