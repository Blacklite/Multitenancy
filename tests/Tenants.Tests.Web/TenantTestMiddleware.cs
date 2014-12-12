using Blacklite.Framework.Multitenancy;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;
using System.Threading.Tasks;

namespace Tenants.Tests.Web
{
    public class TenantTestMiddleware2
    {
        private readonly RequestDelegate _next;

        public TenantTestMiddleware2(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var tenantScoped = httpContext.RequestServices.GetService<TenantDependencyScoped>();
            tenantScoped.Number++;

            var logger = httpContext.RequestServices.GetService<ITenantLogger>();
            logger.WriteWarning(nameof(TenantTestMiddleware2));

            var globalScoped = httpContext.RequestServices.GetService<GlobalDependencyScoped>();
            globalScoped.Number++;

            await _next.Invoke(httpContext);
        }
    }

    public class TenantTestMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantTestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var tenant = httpContext.RequestServices.GetService<ITenant>();

            var tenantSingleton = httpContext.RequestServices.GetService<TenantDependencySingleton>();
            tenantSingleton.Number++;

            var tenantScoped = httpContext.RequestServices.GetService<TenantDependencyScoped>();
            tenantScoped.Number++;

            var tenantTransient = httpContext.RequestServices.GetService<TenantDependencyTransient>();
            tenantTransient.Number++;

            var globalSingleton = httpContext.RequestServices.GetService<GlobalDependencySingleton>();
            globalSingleton.Number++;

            var globalScoped = httpContext.RequestServices.GetService<GlobalDependencyScoped>();
            globalScoped.Number++;

            var globalTransient = httpContext.RequestServices.GetService<GlobalDependencyTransient>();
            globalTransient.Number++;

            var logger = httpContext.RequestServices.GetService<ITenantLogger>();
            logger.WriteWarning(nameof(TenantTestMiddleware));


            await httpContext.Response.WriteAsync(httpContext.Request.Path + "\n\n");

            await httpContext.Response.WriteAsync("Tenant \{tenant.Id}\n");
            await httpContext.Response.WriteAsync("\{nameof(GlobalDependencySingleton)}: \{globalSingleton.Number}\n");
            await httpContext.Response.WriteAsync("\{nameof(GlobalDependencyScoped)}: \{globalScoped.Number}\n");
            await httpContext.Response.WriteAsync("\{nameof(GlobalDependencyTransient)}: \{globalTransient.Number}\n\n");
            await httpContext.Response.WriteAsync("\{nameof(TenantDependencySingleton)}: \{tenantSingleton.Number}\n");
            await httpContext.Response.WriteAsync("\{nameof(TenantDependencyScoped)}: \{tenantScoped.Number}\n");
            await httpContext.Response.WriteAsync("\{nameof(TenantDependencyTransient)}: \{tenantTransient.Number}\n");
            //await httpContext.Response.WriteAsync("Hello world?");

            try
            {
                var appSingleton = httpContext.RequestServices.GetService<ApplicationDependencySingleton>();
            }
            catch
            {
                await httpContext.Response.WriteAsync("Could not fetch \{nameof(ApplicationDependencySingleton)}\n");
            }

            await _next.Invoke(httpContext);
        }

    }
}
