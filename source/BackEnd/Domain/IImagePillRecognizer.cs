using System.Threading.Tasks;

namespace Domain
{
    public interface IImagePillRecognizer
    {
        /// <summary>
        /// This will check if the image is a pill.
        /// Since it is not a complete classification the image will be deleted from the predicted images
        /// </summary>
        Task<bool> IsPill(byte[] image);
    }
}
