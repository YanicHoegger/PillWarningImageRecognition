using CustomVisionInteraction.Interface;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CustomVisionInteraction.Prediction
{
    public class PredictionResult : IPredictionResult
    {
        public PredictionResult(bool success, IEnumerable<string> tags, Color color)
        {
            Success = success;
            Tags = tags;
            Color = color;
        }

        public bool Success { get; }
        public IEnumerable<string> Tags { get; }
        public Color Color { get; }

        public static PredictionResult FromSuccess(IEnumerable<string> tags, Color color)
        {
            return new PredictionResult(true, tags, color);
        }

        public static PredictionResult GetNoSuccess()
        {
            return new PredictionResult(false, Enumerable.Empty<string>(), Color.Empty);
        }
    }
}
