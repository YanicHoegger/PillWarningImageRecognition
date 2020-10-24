using System.Collections.Generic;

namespace ImageInteraction.PredictedImagesManager.Dtos
{
    public class PredictedImagesDto
    {
        public List<PredictedImageDto> Results { get; set; }
        public TokenDto Token { get; set; }
    }
}
