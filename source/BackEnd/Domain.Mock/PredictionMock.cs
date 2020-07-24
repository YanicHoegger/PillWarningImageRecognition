using Domain.Interface;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

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
            var converterFactory = CreateConverterFactory();

            var serializeOptions = new JsonSerializerOptions
            {
                Converters = { converterFactory }
            };

            _resultValue = JsonSerializer.Deserialize<PredictionResultMock>(Resources.WarnerBrothers, serializeOptions);

            return Task.CompletedTask;
        }

        private static InterfaceImplementationJsonConverterFactory CreateConverterFactory()
        {
            var converterFactory = new InterfaceImplementationJsonConverterFactory();

            converterFactory.Register<IFinding, FindingMock>();
            converterFactory.Register<IPillWarning, PillWarningMock>();
            converterFactory.Register<IPillWarningInfo, PillWarningInfoMock>();

            return converterFactory;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
