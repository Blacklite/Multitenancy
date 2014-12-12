using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [ServiceDescriptor(Lifecycle = LifecycleKind.Singleton)]
    public class GlobalDependencySingleton
    {
        public int Number { get; set; }

        public GlobalDependencySingleton()
        {
        }
    }

    [ServiceDescriptor(Lifecycle = LifecycleKind.Scoped)]
    public class GlobalDependencyScoped
    {
        public int Number { get; set; }

        public GlobalDependencyScoped()
        {
        }
    }

    [ServiceDescriptor(Lifecycle = LifecycleKind.Transient)]
    public class GlobalDependencyTransient
    {
        public int Number { get; set; }

        public GlobalDependencyTransient()
        {
        }
    }
}
