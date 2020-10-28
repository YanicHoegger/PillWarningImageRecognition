using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ImageInteraction.PredictedImagesManager
{
    public class CachedPredictedImagesProvider : PredictedImagesProviderBase, IHostedService
    {
        private readonly List<PredictedImage> _predictedImages = new List<PredictedImage>();

        public CachedPredictedImagesProvider(IContext context) 
            : base(context)
        {
        }

        public override IAsyncEnumerable<PredictedImage> GetPredictedImages()
        {
            return _predictedImages.ToAsyncEnumerable();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _predictedImages.AddRange(await GetPredictedImagesInternal(cancellationToken).ToListAsync(cancellationToken));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
