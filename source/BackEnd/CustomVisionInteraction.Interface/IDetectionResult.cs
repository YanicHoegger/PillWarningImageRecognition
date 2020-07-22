﻿namespace CustomVisionInteraction.Interface
{
    public interface IDetectionResult
    {
        IBoundingBox BoundingBox { get; }
        double Probability { get; }
    }
}
