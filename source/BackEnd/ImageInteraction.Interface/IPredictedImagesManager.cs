using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IPredictedImagesManager
    {
        Task DeletePredictedImage(byte[] image);

        IAsyncEnumerable<IPredictedImage> GetPredictedImages();
    }
}
