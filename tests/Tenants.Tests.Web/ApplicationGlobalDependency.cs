using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [ApplicationOnly]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Singleton)]
    public class ApplicationDependencySingleton
    {
        public int Number { get; set; }

        public ApplicationDependencySingleton()
        {
        }
    }

    [ApplicationOnly]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Scoped)]
    public class ApplicationDependencyScoped
    {
        public int Number { get; set; }

        public ApplicationDependencyScoped()
        {
        }
    }

    [ApplicationOnly]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Transient)]
    public class ApplicationDependencyTransient
    {
        public int Number { get; set; }

        public ApplicationDependencyTransient()
        {
        }
    }
}
