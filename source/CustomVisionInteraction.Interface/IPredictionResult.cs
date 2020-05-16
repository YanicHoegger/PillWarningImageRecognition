using System.Collections.Generic;
using System.Drawing;

namespace CustomVisionInteraction.Interface
{
    public interface IPredictionResult
    {
        bool Success { get; }
        IEnumerable<string> Tags { get; }
        Color Color { get; }
    }
}
