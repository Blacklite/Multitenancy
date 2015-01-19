using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public class TenantServicesContainer : IDisposable
    {
        private HttpContext _context { get; set; }
        private IServiceProvider _priorAppServices { get; set; }
        private IServiceProvider _priorRequestServices { get; set; }
        private ITenantScope _scope { get; set; }

        public TenantServicesContainer(
            HttpContext context,
            IServiceProvider appServiceProvider,
            ITenantProvider scopeFactory,
            string tenantId)
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
            _priorAppServices = context.ApplicationServices;
            _priorRequestServices = context.RequestServices;

            var scope = _scope = scopeFactory.GetOrAdd(tenantId);

            if (scope.Tenant.State == TenantState.Boot)
                scope.Tenant.DoStart();

            _context.ApplicationServices = appServiceProvider;
            _context.RequestServices = _scope.ServiceProvider;
        }


        // CONSIDER: this could be an extension method on HttpContext instead
        public static TenantServicesContainer EnsureTenantServices(HttpContext httpContext, IServiceProvider services, string tenantId)
        {
            // All done if we already have a request services
            if (httpContext.RequestServices != null)
            {
                return null;
            }

            var serviceProvider = httpContext.ApplicationServices ?? services;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("TODO: services and httpContext.ApplicationServices are both null!");
            }

            // Matches constructor of RequestContainer
            var rootServiceProvider = serviceProvider.GetRequiredService<IServiceProvider>();
            var rootServiceScopeFactory = serviceProvider.GetRequiredService<ITenantProvider>();

            // Pre Scope setup
            var priorApplicationServices = serviceProvider;
            var priorRequestServices = serviceProvider;

            var appServiceProvider = rootServiceProvider;
            var appServiceScopeFactory = rootServiceScopeFactory;

            if (priorApplicationServices != null &&
                priorApplicationServices != appServiceProvider)
            {
                appServiceProvider = priorApplicationServices;
                appServiceScopeFactory = priorApplicationServices.GetRequiredService<ITenantProvider>();
            }

            // Creates the scope and does the service swaps
            return new TenantServicesContainer(httpContext, appServiceProvider, appServiceScopeFactory, tenantId);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.RequestServices = _priorRequestServices;
                    _context.ApplicationServices = _priorAppServices;
                }

                if (_scope != null)
                {
                    _scope.Dispose();
                    _scope = null;
                }

                _context = null;
                _priorAppServices = null;
                _priorRequestServices = null;

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
