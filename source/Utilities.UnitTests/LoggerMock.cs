using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Utilities.UnitTests
{
    public class LoggerMock<T> : ILogger<T>
    {
        private readonly List<string> _logged = new List<string>();

        public IEnumerable<string> Logged => _logged;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logged.Add(formatter(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoDisposing();
        }
    }

    public class NoDisposing : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
