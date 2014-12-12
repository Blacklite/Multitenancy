using Autofac;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    class AutofacServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacServiceScopeFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public IServiceScope CreateScope()
        {
            return new AutofacServiceScope(_lifetimeScope.BeginLifetimeScope());
        }
    }

    class AutofacServiceScope : IServiceScope
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IServiceProvider _serviceProvider;

        public AutofacServiceScope(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _serviceProvider = _lifetimeScope.Resolve<IServiceProvider>();
        }

        public IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}
