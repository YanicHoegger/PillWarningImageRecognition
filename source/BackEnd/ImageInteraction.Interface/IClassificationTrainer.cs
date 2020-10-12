using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IClassificationTrainer
    {
        //TODO: one should not use tuples in interface assemblies
        Task Train(IEnumerable<(byte[] image, string tag)> inputData);
    }
}
