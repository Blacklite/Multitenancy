using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    class TenantRequestServicesContainer : IDisposable
    {
        private HttpContext _context;
        private IServiceProvider _priorAppServices;
        private HttpContext _priorAppHttpContext;
        private HttpContext _priorScopeHttpContext;
        private IServiceScope _scope;
        private IContextAccessor<HttpContext> _httpContextAccessor;
        private IContextAccessor<HttpContext> _scopeContextAccessor;

        public TenantRequestServicesContainer(
            HttpContext context,
            string tenantId,
            IContextAccessor<HttpContext> httpContextAccessor,
            ITenantProvider scopeFactory,
            IServiceProvider appServiceProvider)
        {
            if (scopeFactory == null)
            {
                throw new ArgumentNullException(nameof(scopeFactory));
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _priorAppServices = context.ApplicationServices;

            // Begin the scope
            _scope = scopeFactory.GetOrCreateScope(tenantId);
            _scopeContextAccessor = _scope.ServiceProvider.GetRequiredService<IContextAccessor<HttpContext>>();

            _context.ApplicationServices = _scope.ServiceProvider;

            _priorAppHttpContext = _httpContextAccessor.SetValue(context);
            _priorScopeHttpContext = _scopeContextAccessor.SetValue(context);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpContextAccessor.SetValue(_priorAppHttpContext);
                    _scopeContextAccessor.SetValue(_priorScopeHttpContext);
                    _context.ApplicationServices = _priorAppServices;
                }

                _context = null;
                _priorAppServices = null;
                _scopeContextAccessor = null;
                _priorAppHttpContext = null;
                _priorScopeHttpContext = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}