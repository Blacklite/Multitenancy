using System;
#if ASPNET50
using System.Runtime.Remoting.Messaging;
#endif
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Hosting;
#if ASPNET50
using System.Runtime.Remoting;
#endif

namespace Blacklite.Framework.Multitenancy
{
    public class TenantContainerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly IContextAccessor<HttpContext> _httpContextAccessor;
        private readonly ITenantProvider _serviceScopeFactory;
        private readonly ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private readonly IHostingEnvironment _environment;

        public TenantContainerMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider,
            ITenantProvider serviceScopeFactory,
            ITenantIdentificationStrategy tenantIdentificationStrategy,
            IHostingEnvironment environment,
            IContextAccessor<HttpContext> httpContextAccessor)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("rootServiceProvider");
            }
            if (serviceScopeFactory == null)
            {
                throw new ArgumentNullException("rootServiceScopeFactory");
            }

            _next = next;
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _environment = environment;

            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.RequestServices != null)
            {
                throw new Exception("TODO: nested request container scope? this is probably a mistake on your part?");
            }

            var priorApplicationServices = httpContext.ApplicationServices;
            var priorRequestServices = httpContext.RequestServices;

            var appServiceProvider = _serviceProvider;
            var appServiceScopeFactory = _serviceScopeFactory;
            var appHttpContextAccessor = _httpContextAccessor;

            if (priorApplicationServices != null &&
                priorApplicationServices != appServiceProvider)
            {
                appServiceProvider = priorApplicationServices;
                appServiceScopeFactory = priorApplicationServices.GetRequiredService<ITenantProvider>();
                appHttpContextAccessor = priorApplicationServices.GetRequiredService<IContextAccessor<HttpContext>>();
            }

            string tenantId;
            if (!_tenantIdentificationStrategy.TryIdentifyTenant(httpContext, out tenantId))
                throw new Exception("Could not identify tenant!");

            using (var container = new TenantRequestServicesContainer(httpContext, tenantId, appHttpContextAccessor, appServiceScopeFactory, appServiceProvider))
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}