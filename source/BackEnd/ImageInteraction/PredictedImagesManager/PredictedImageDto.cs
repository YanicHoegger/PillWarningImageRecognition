using System.Collections.Generic;

namespace ImageInteraction.PredictedImagesManager
{
    public class PredictedImageDto
    {
        public string OriginalImageUri { get; set; }

        public List<PredictionResultDto> Predictions { get; set; }

        public string Id { get; set; }
    }
}
