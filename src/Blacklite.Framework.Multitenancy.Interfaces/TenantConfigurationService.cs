#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [ApplicationOnly]
    public interface ITenantConfigurationService
    {
        void Configure(ITenant tenant);
    }
}
