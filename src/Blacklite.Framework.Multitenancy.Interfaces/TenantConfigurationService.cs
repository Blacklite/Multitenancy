using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.Configuration
{    [ApplicationOnly]
    public interface ITenantConfigurationService
    {
        void Configure(ITenant tenant);
    }
}
