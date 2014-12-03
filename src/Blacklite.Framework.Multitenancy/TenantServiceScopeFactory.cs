﻿using Autofac;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantServiceScopeFactory
    {
        IServiceScope GetOrCreateScope(string tenantIdentifier);
    }

    class TenantServiceScopeFactory : ITenantServiceScopeFactory
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, IServiceScope> _tenantScopes = new ConcurrentDictionary<string, IServiceScope>();

        public TenantServiceScopeFactory(ILifetimeScope lifetimeScope, IServiceProvider serviceProvider)
        {
            _lifetimeScope = lifetimeScope;
            _serviceProvider = serviceProvider;
        }

        public IServiceScope GetOrCreateScope(string tenantIdentifier)
        {
            return _tenantScopes.GetOrAdd(tenantIdentifier, x => new TenantServiceScope(_lifetimeScope.BeginLifetimeScope(AutofacTenantRegistration.TenantTag), x));
        }
    }

    class TenantServiceScope : IServiceScope
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IServiceProvider _serviceProvider;

        public TenantServiceScope(ILifetimeScope lifetimeScope, string tenantIdentifier)
        {
            _lifetimeScope = lifetimeScope;
            _serviceProvider = _lifetimeScope.Resolve<IServiceProvider>();

            var tenant = _serviceProvider.GetRequiredService<ITenant>();
            tenant.Identifier = tenantIdentifier;
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