using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantLogger : ILogger { }

    [LifecyclePerTenant]
    class TenantLogger : ITenantLogger
    {
        private ILogger _logger;
        public TenantLogger([NotNull] ITenant tenant, [NotNull] ILoggerFactory factory)
        {
            _logger = factory.Create(tenant.Id);
        }

        public IDisposable BeginScope(object state)
        {
            return _logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Write(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            _logger.Write(logLevel, eventId, state, exception, formatter);
        }
    }
}
