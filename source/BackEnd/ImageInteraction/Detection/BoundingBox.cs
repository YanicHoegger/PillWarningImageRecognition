using ImageInteraction.Interface;
using CognitiveServices = Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ImageInteraction.Detection
{
    public class BoundingBox : IBoundingBox
    {
        public BoundingBox(CognitiveServices.BoundingBox boundingBox)
            : this(boundingBox.Left, boundingBox.Top, boundingBox.Width, boundingBox.Height)
        {
        }

        public BoundingBox(double left, double top, double width, double height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public double Left { get; }
        public double Top { get; }
        public double Width { get; }
        public double Height { get; }
    }
}
