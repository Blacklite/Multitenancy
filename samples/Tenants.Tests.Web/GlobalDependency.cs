using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [ServiceDescriptor(Lifetime = ServiceLifetime.Singleton)]
    public class GlobalDependencySingleton
    {
        public int Number { get; set; }

        public GlobalDependencySingleton()
        {
        }
    }

    [ServiceDescriptor(Lifetime = ServiceLifetime.Scoped)]
    public class GlobalDependencyScoped
    {
        public int Number { get; set; }

        public GlobalDependencyScoped()
        {
        }
    }

    [ServiceDescriptor(Lifetime = ServiceLifetime.Transient)]
    public class GlobalDependencyTransient
    {
        public int Number { get; set; }

        public GlobalDependencyTransient()
        {
        }
    }
}
