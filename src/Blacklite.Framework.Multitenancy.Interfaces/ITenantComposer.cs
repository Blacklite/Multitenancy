using System;

namespace Blacklite.Framework.Multitenancy
{
    [ApplicationOnly]
    public interface ITenantComposer
    {
        int Order { get; }
        void Configure(ITenant tenant);
    }
}
