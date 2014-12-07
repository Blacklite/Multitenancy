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

            var applicationScoped = httpContext.RequestServices.GetService<ApplicationDependencyScoped>();
            applicationScoped.Number++;

            _next.Invoke(httpContext);
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

            var applicationSingleton = httpContext.RequestServices.GetService<ApplicationDependencySingleton>();
            applicationSingleton.Number++;

            var applicationScoped = httpContext.RequestServices.GetService<ApplicationDependencyScoped>();
            applicationScoped.Number++;

            var applicationTransient = httpContext.RequestServices.GetService<ApplicationDependencyTransient>();
            applicationTransient.Number++;

            var logger = httpContext.RequestServices.GetService<ITenantLogger>();
            logger.WriteWarning(nameof(TenantTestMiddleware));


            await httpContext.Response.WriteAsync(httpContext.Request.Path + "\n\n");

            await httpContext.Response.WriteAsync("Tenant \{tenant.Id}\n");
            await httpContext.Response.WriteAsync("ApplicationDependencySingleton: \{applicationSingleton.Number}\n");
            await httpContext.Response.WriteAsync("ApplicationDependencyScoped: \{applicationScoped.Number}\n");
            await httpContext.Response.WriteAsync("ApplicationDependencyTransient: \{applicationTransient.Number}\n\n");
            await httpContext.Response.WriteAsync("TenantDependencySingleton: \{tenantSingleton.Number}\n");
            await httpContext.Response.WriteAsync("TenantDependencyScoped: \{tenantScoped.Number}\n");
            await httpContext.Response.WriteAsync("TenantDependencyTransient: \{tenantTransient.Number}\n");
            //await httpContext.Response.WriteAsync("Hello world?");
            //await _next.Invoke(httpContext);
        }

    }
}