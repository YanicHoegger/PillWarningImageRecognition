using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IPredictedImagesManager
    {
        Task DeletePredictedImages(IEnumerable<byte[]> images);

        IEnumerable<IPredictedImage> GetPredictedImages();
    }
}
