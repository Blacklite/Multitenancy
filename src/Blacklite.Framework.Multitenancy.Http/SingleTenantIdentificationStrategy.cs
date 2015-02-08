using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class SingleTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public ITenantIdentificationResult TryIdentifyTenant(HttpContext context)
        {
            return TenantIdentificationResult.Passed;
        }

        public Task<ITenantIdentificationResult> TryIdentifyTenantAsync([NotNull]HttpContext context)
        {
            return Task.FromResult(TryIdentifyTenant(context));
        }
    }
}
