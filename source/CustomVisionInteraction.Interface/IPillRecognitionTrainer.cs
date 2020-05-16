using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IPillRecognitionTrainer
    {
        Task Train(IEnumerable<(byte[] image, string tag)> inputData);
    }
}
