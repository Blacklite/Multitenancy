using System;

namespace Blacklite.Framework.Multitenancy.Configurators
{
    public class ApplicationBroadcastConfigurator : ITenantComposer
    {
        public int Order { get; } = 0;

        public void Configure(ITenant tenant)
        {
            tenant.
        }
    }
}
