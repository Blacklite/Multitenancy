using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{    [ApplicationOnly]
    public interface ITenantConfigurationService
    {
        void Configure(ITenant tenant);
    }
}
