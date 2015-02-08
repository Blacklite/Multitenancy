using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Framework.ConfigurationModel;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class TenantContainerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _services;
        private readonly ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private readonly ITenantProvider _tenantProvider;

        public TenantContainerMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider,
            ITenantProvider tenantProvider,
            ITenantIdentificationStrategy tenantIdentificationStrategy)
        {
            if (serviceProvider == null) { throw new ArgumentNullException(nameof(serviceProvider)); }
            if (tenantProvider == null) { throw new ArgumentNullException(nameof(tenantProvider)); }
            if (tenantIdentificationStrategy == null) { throw new ArgumentNullException(nameof(tenantIdentificationStrategy)); }

            _next = next;
            _services = serviceProvider;
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _tenantProvider = tenantProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var tenantIdentificationResult = await _tenantIdentificationStrategy.TryIdentifyTenantAsync(httpContext);
            if (!tenantIdentificationResult.Success)
                if (string.IsNullOrWhiteSpace(tenantIdentificationResult.Id))
                    throw new Exception("Could not identify tenant!");
                else
                    throw new InvalidOperationException($"The tenant {tenantIdentificationResult.Id} is not currently active.");

            var scope = _tenantProvider.GetOrAdd(tenantIdentificationResult.Id);

            if (scope.Tenant.State == TenantState.Boot)
                await scope.Tenant.DoStart();

            httpContext.ApplicationServices = scope.ServiceProvider.GetRequiredService<IServiceProvider>();

            await _next(httpContext);
        }
    }
}
