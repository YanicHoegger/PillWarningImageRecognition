using System.Collections.Generic;
using System.Threading;

namespace ImageInteraction.PredictedImagesManager
{
    public class PredictedImagesProvider : PredictedImagesProviderBase
    {
        public PredictedImagesProvider(IContext context) 
            : base(context)
        {
        }

        public override IAsyncEnumerable<PredictedImage> GetPredictedImages()
        {
            return GetPredictedImagesInternal(CancellationToken.None);
        }
    }
}
