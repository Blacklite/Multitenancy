using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Framework.Configuration;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private readonly ITenantProvider _tenantProvider;
        private readonly TenantMiddlewareOptions _options;

        public TenantMiddleware(
            RequestDelegate next,
            ITenantProvider tenantProvider,
            ITenantIdentificationStrategy tenantIdentificationStrategy,
            TenantMiddlewareOptions options)
        {
            if (tenantProvider == null) { throw new ArgumentNullException(nameof(tenantProvider)); }
            if (tenantIdentificationStrategy == null) { throw new ArgumentNullException(nameof(tenantIdentificationStrategy)); }

            _next = next;
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _tenantProvider = tenantProvider;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var tenantIdentificationResult = await _tenantIdentificationStrategy.IdentifyTenantAsync(httpContext);
            if (tenantIdentificationResult.Enabled && tenantIdentificationResult.Success)
            {
                var scope = _tenantProvider.GetOrAdd(tenantIdentificationResult.Id);

                if (scope.Tenant.State == TenantState.Boot)
                    await scope.Tenant.DoStart();

                httpContext.ApplicationServices = scope.ServiceProvider.GetRequiredService<IServiceProvider>();
            }

            if (!tenantIdentificationResult.Success && _options.ThrowWhenFailed)
                throw new Exception("Could not identify tenant!");

            if (tenantIdentificationResult.Success && !tenantIdentificationResult.Success && _options.ThrowWhenOffline)
                throw new InvalidOperationException($"The tenant {tenantIdentificationResult.Id} is not currently active.");

            await _next(httpContext);
        }
    }
}
