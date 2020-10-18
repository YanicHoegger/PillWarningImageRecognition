using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IClassificationTrainer
    {
        Task Train(IEnumerable<ITrainingImage> trainingImages);
    }
}
