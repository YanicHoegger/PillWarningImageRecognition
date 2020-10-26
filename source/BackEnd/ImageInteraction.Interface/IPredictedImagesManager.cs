using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IPredictedImagesManager
    {
        Task DeletePredictedImages(byte[] image);

        IEnumerable<IPredictedImage> GetPredictedImages();
    }
}
