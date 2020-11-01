using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Utilities.Aspects
{
    public class LoggingDecorator<T> : DispatchProxy
    {
        private T _decorated;
        private ILogger<T> _logger;

        public static T Create(T decorated, ILogger<T> logger)
        {
            object proxy = Create<T, LoggingDecorator<T>>();

            ((LoggingDecorator<T>)proxy).SetParameters(decorated, logger);

            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                _logger.LogDebug($"Enter method {targetMethod.Name}");
                var result = targetMethod.Invoke(_decorated, args);
                _logger.LogDebug($"Leaving method {targetMethod.Name}");

                return result;
            }
            catch (TargetInvocationException e)
            {
                var innerException = e.InnerException;
                if (innerException != null)
                {
                    _logger.LogError($"Method {targetMethod.Name} threw exception:{Environment.NewLine}{innerException.Message}");
                    throw innerException;
                }

                _logger.LogError($"Method {targetMethod.Name} threw exception:{Environment.NewLine}{e.Message}");
                throw e;
            }
        }

        private void SetParameters(T decorated, ILogger<T> logger)
        {
            _decorated = decorated;
            _logger = logger;
        }
    }
}
