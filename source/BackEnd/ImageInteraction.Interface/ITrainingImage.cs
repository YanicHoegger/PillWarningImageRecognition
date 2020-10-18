using System.Collections.Generic;

namespace ImageInteraction.Interface
{
    public interface ITrainingImage
    {
        byte[] Image { get; }
        IEnumerable<string> Tags { get; }
    }
}
