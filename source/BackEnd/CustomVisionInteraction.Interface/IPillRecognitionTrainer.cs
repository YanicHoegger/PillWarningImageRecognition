using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IPillRecognitionTrainer
    {
        //TODO: one should not use tuples in interface assemblies
        Task Train(IEnumerable<(byte[] image, string tag)> inputData);
    }
}
