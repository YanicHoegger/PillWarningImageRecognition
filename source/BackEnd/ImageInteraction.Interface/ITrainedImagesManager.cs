using System.Collections.Generic;

namespace ImageInteraction.Interface
{
    public interface ITrainedImagesManager
    {
        IAsyncEnumerable<byte[]> GetTrainedImages();
    }
}
