#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using Microsoft.Framework.ConfigurationModel;
using System;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [TenantOnly]
    public interface ITenantConfiguration : IConfigurationSourceContainer
    {

    }
}
