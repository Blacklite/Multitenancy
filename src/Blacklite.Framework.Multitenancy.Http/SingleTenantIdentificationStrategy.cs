using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class SingleTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public TenantIdentificationResult IdentifyTenant(HttpContext context)
        {
            return TenantIdentificationResult.Passed;
        }

        public Task<TenantIdentificationResult> IdentifyTenantAsync([NotNull]HttpContext context)
        {
            return Task.FromResult(IdentifyTenant(context));
        }
    }
}
