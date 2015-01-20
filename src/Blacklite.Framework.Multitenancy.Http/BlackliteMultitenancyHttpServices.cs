using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy
{
    public static class BlackliteMultitenancyHttpServices
    {
        public static IEnumerable<IServiceDescriptor> GetDefaultServices(IConfiguration configuration = null)
        {
            return Enumerable.Empty<IServiceDescriptor>();
        }
    }
}
