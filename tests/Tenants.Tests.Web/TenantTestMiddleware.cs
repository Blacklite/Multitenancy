using Blacklite.Framework.Multitenancy;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Tenants.Tests.Web
{
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
            var tenantDep1 = httpContext.RequestServices.GetService<TenantDependency>();
            var tenantDep2 = httpContext.RequestServices.GetService<TenantDependency2>();

            await httpContext.Response.WriteAsync(tenant.Identifier + "\n");
            await httpContext.Response.WriteAsync(tenantDep1.Number.ToString() + "\n");
            await httpContext.Response.WriteAsync(tenantDep2.Number.ToString() + "\n");
            //await httpContext.Response.WriteAsync("Hello world?");
            //await _next.Invoke(httpContext);
        }

    }
}