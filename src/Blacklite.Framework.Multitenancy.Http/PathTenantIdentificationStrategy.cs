using Microsoft.AspNet.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class PathTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private readonly ITenantRegistry _tenantRegistry;
        public PathTenantIdentificationStrategy(ITenantRegistry tenantRegistry)
        {
            _tenantRegistry = tenantRegistry;
        }

        private static object _tenantKey = new object();

        public ITenantIdentificationResult TryIdentifyTenant(HttpContext context)
        {
            var task = TryIdentifyTenantAsync(context);
            task.Wait();
            return task.Result;
        }

        public async Task<ITenantIdentificationResult> TryIdentifyTenantAsync([NotNull]HttpContext context)
        {
            if (context.Items.ContainsKey(_tenantKey))
            {
                return (ITenantIdentificationResult)context.Items[_tenantKey];
            }

            ITenantIdentificationResult result;
            if (context.Request.Path.HasValue && context.Request.Path.Value.Count(x => x == '/') > 0)
            {
                var path = context.Request.Path.Value.TrimStart('/');
                var name = path.Split('/')[0];

                if (!string.IsNullOrWhiteSpace(name))
                {
                    var registry = await _tenantRegistry.GetTenantItemAsync(name);
                    if (registry.IsEnabled)
                    {
                        result = new TenantIdentificationResult(registry.Id);
                    }
                    else
                    {
                        // We still know the tenant ID, the other party might be interested to know that information
                        result = new TenantIdentificationResult(registry.Id, false);
                    }

                    context.Items[_tenantKey] = result;

                    var tenantPath = new PathString(string.Format("/{0}", result.Id));
                    PathString remainingPath;
                    if (context.Request.Path.StartsWithSegments(tenantPath, out remainingPath))
                    {
                        // Move the request path so it appears as if it was rooted like normal.
                        PathString pathBase = context.Request.PathBase;
                        context.Request.PathBase = pathBase + tenantPath;
                        context.Request.Path = remainingPath;
                    }

                    return result;
                }
            }

            result = TenantIdentificationResult.Failed;
            context.Items[_tenantKey] = result;
            return result;
        }
    }
}
