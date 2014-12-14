using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface ITenantConfigurationService
    {
        void Configure(ITenant tenant);
    }
}
