using System.Collections.Generic;

namespace ImageInteraction.Interface
{
    public interface IImageClassificationResult
    {
        IEnumerable<ITagClassificationResult> TagClassifications { get; }
    }
}
