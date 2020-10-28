using System.Threading.Tasks;

namespace Domain
{
    public interface IImagePillRecognizer
    {
        /// <summary>
        /// This will check if the image is a pill.
        /// The classification result will not be stored
        /// </summary>
        Task<bool> IsPill(byte[] image);
    }
}
