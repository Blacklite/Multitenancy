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
        public async Task Invoke(HttpContext httpContext, TenantDependencyScoped tenantScoped, ITenantLogger logger, GlobalDependencyScoped globalScoped)
        {
            tenantScoped.Number++;
            logger.WriteWarning(nameof(TenantTestMiddleware2));
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

        public async Task Invoke(HttpContext httpContext, ITenant tenant,
            TenantDependencySingleton tenantSingleton, TenantDependencyScoped tenantScoped, TenantDependencyTransient tenantTransient,
            ITenantLogger logger,
            GlobalDependencySingleton globalSingleton, GlobalDependencyScoped globalScoped, GlobalDependencyTransient globalTransient
        )
        {
            tenantSingleton.Number++;
            tenantScoped.Number++;
            tenantTransient.Number++;

            globalSingleton.Number++;
            globalScoped.Number++;
            globalTransient.Number++;

            logger.WriteWarning(nameof(TenantTestMiddleware));

            await httpContext.Response.WriteAsync(httpContext.Request.Path + "\n\n");

            await httpContext.Response.WriteAsync(string.Format("Tenant {0}\n", tenant.Id));
            await httpContext.Response.WriteAsync(nameof(GlobalDependencySingleton) + ": " + globalSingleton.Number + "\n");
            await httpContext.Response.WriteAsync(nameof(GlobalDependencyScoped) + ": " + globalScoped.Number + "\n");
            await httpContext.Response.WriteAsync(nameof(GlobalDependencyTransient) + ": " + globalTransient.Number + "\n\n");
            await httpContext.Response.WriteAsync(nameof(TenantDependencySingleton) + ": " + tenantSingleton.Number + "\n");
            await httpContext.Response.WriteAsync(nameof(TenantDependencyScoped) + ": " + tenantScoped.Number + "\n");
            await httpContext.Response.WriteAsync(nameof(TenantDependencyTransient) + ": " + tenantTransient.Number + "\n");
            //await httpContext.Response.WriteAsync("Hello world?");

            try
            {
                var appSingleton = httpContext.RequestServices.GetService<ApplicationDependencySingleton>();
            }
            catch
            {
                await httpContext.Response.WriteAsync("Could not fetch " + nameof(ApplicationDependencySingleton) + "\n");
            }

            await _next.Invoke(httpContext);
        }

    }
}
