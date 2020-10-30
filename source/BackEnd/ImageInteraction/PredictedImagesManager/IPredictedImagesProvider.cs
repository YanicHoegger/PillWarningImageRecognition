using System.Collections.Generic;

namespace ImageInteraction.PredictedImagesManager
{
    public interface IPredictedImagesProvider
    {
        IAsyncEnumerable<PredictedImage> GetPredictedImages();
    }
}
