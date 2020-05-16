using Domain.Interface;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Mock
{
    public class PredictionMock : IPredicition, IHostedService
    {
        private PredictionResultMock _resultValue;

        public Task<IPredictionResult> Predict(byte[] image)
        {
            return Task.FromResult<IPredictionResult>(_resultValue);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _resultValue = JsonSerializer.Deserialize<PredictionResultMock>(Resources.OnlyInprintWarnerBrothers);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
