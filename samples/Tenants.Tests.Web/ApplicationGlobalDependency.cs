using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [ApplicationOnly]
    [ServiceDescriptor(Lifetime = ServiceLifetime.Singleton)]
    public class ApplicationDependencySingleton
    {
        public int Number { get; set; }

        public ApplicationDependencySingleton()
        {
        }
    }

    [ApplicationOnly]
    [ServiceDescriptor(Lifetime = ServiceLifetime.Scoped)]
    public class ApplicationDependencyScoped
    {
        public int Number { get; set; }

        public ApplicationDependencyScoped()
        {
        }
    }

    [ApplicationOnly]
    [ServiceDescriptor(Lifetime = ServiceLifetime.Transient)]
    public class ApplicationDependencyTransient
    {
        public int Number { get; set; }

        public ApplicationDependencyTransient()
        {
        }
    }
}
