using System;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Utilities.Aspects
{
    public class HostedServiceDecorator<T> : DispatchProxy
    {
        private T _decorated;
        private IHostedServiceState _hostedServiceState;

        public static (T, HostedServiceState<THosted>) Create<THosted>(THosted decorated, ILogger<THosted> logger)
            where THosted : T, IHostedService
        {
            object proxy = Create<T, HostedServiceDecorator<T>>();
            var hostedServiceState = new HostedServiceState<THosted>(decorated, logger);

            ((HostedServiceDecorator<T>)proxy).SetParameters(decorated, hostedServiceState);

            return ((T)proxy, hostedServiceState);
        }

        private void SetParameters(T decorated, IHostedServiceState hostedServiceState)
        {
            _decorated = decorated;
            _hostedServiceState = hostedServiceState;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if(!_hostedServiceState.IsStarted)
                throw new InvalidOperationException($"{typeof(T).GetFriendlyName()} needs to be started first");

            return targetMethod.Invoke(_decorated, args);
        }
    }
}
