using Blacklite.Framework.Multitenancy.Configuration;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.Http
{
    public static class BlackliteMultitenancyHttpServices
    {
        public static IEnumerable<ServiceDescriptor> GetDefaultServices()
        {
            return Enumerable.Empty<ServiceDescriptor>();
        }
    }
}
