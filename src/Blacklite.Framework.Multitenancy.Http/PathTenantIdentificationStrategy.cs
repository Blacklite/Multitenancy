using Microsoft.AspNet.Http;
using System;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class PathTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private static object _tenantId = new object();

        public bool TryIdentifyTenant(HttpContext context, out string tenantId)
        {
            if (context.Items.ContainsKey(_tenantId))
            {
                tenantId = (string)context.Items[_tenantId];
                return true;
            }

            if (context.Request.Path.HasValue && context.Request.Path.Value.Count(x => x == '/') > 0)
            {
                var path = context.Request.Path.Value.TrimStart('/');
                var name = path.Split('/')[0];
                if (!string.IsNullOrWhiteSpace(name) && !name.Contains("."))
                {
                    context.Items[_tenantId] = tenantId = name;
                    return true;
                }
            }

            tenantId = null;
            return false;
        }
    }
}
