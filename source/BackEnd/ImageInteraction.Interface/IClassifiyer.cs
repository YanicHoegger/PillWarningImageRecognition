using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IClassifier
    {
        Task<IImageClassificationResult> GetImageClassification(byte[] image);
    }
}
