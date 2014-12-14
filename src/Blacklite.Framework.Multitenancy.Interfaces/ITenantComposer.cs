using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface ITenantComposer
    {
        int Order { get; }
        void Configure(ITenant tenant);
    }
}
