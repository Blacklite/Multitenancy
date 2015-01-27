using Microsoft.AspNet.Http;
using System;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class SingleTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public bool TryIdentifyTenant(HttpContext context, out string tenantId)
        {
            tenantId = "Single";
            return true;
        }
    }
}
