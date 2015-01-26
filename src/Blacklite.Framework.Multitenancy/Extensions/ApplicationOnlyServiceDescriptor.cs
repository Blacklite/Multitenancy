﻿using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public class ApplicationOnlyServiceDescriptor : ServiceDescriptor
    {
        public ApplicationOnlyServiceDescriptor([NotNull]Type serviceType, [NotNull]Func<IServiceProvider, object> factory, LifecycleKind lifecycle)
            : base(serviceType, factory, lifecycle)
        {

        }

        public ApplicationOnlyServiceDescriptor([NotNull]Type serviceType, [NotNull]Type implementationType, LifecycleKind lifecycle)
            : base(serviceType, implementationType, lifecycle)
        {

        }
    }
}