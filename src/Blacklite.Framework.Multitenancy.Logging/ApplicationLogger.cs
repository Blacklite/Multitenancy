using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;

namespace Blacklite.Framework.Multitenancy
{
    class ApplicationLogger : IApplicationLogger
    {
        private ILogger _logger;
        public ApplicationLogger([NotNull] ILoggerFactory factory)
        {
            _logger = factory.CreateLogger("Application");
        }

        public IDisposable BeginScopeImpl(object state)
        {
            return _logger.BeginScopeImpl(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
