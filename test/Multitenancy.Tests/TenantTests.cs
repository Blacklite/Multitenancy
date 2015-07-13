using Microsoft.Framework.Configuration;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Configuration;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;

namespace Multitenancy.Tests
{
    public class TenantTests
    {
        [Fact]
        public void IntializeRunsOnlyOnce()
        {
            var tenant = new Tenant(new TenantConfiguration(new List<IConfigurationSource>()));

            Assert.Null(tenant.Id);

            tenant.Initialize("tenant1", Mock.Of<IServiceProvider>());

            Assert.Equal(tenant.Id, "tenant1");

            tenant.Initialize("tenant2", Mock.Of<IServiceProvider>());

            Assert.NotEqual(tenant.Id, "tenant2");
        }

        [Fact]
        public void TenantHasNoStateWhenCreated()
        {
            var tenant = new Tenant(new TenantConfiguration(new List<IConfigurationSource>()));
            tenant.Initialize("tenant1", Mock.Of<IServiceProvider>());

            Assert.Equal(tenant.State, TenantState.None);
        }

        [Fact]
        public void TenantChangesStateWhenBooted()
        {
            var tenant = new Tenant(new TenantConfiguration(new List<IConfigurationSource>()));
            tenant.Initialize("tenant1", Mock.Of<IServiceProvider>());

            tenant.ChangeState(TenantState.Boot);

            Assert.Equal(tenant.State, TenantState.Boot);
        }

        [Fact]
        public void TenantChangesStateWhenStarted()
        {
            var tenant = new Tenant(new TenantConfiguration(new List<IConfigurationSource>()));
            tenant.Initialize("tenant1", Mock.Of<IServiceProvider>());

            tenant.ChangeState(TenantState.Started);

            Assert.Equal(tenant.State, TenantState.Started);
        }

        [Fact]
        public void TenantIgnoresChangesStateWhenStoppedUntilStarted()
        {
            var tenant = new Tenant(new TenantConfiguration(new List<IConfigurationSource>()));
            tenant.Initialize("tenant1", Mock.Of<IServiceProvider>());

            tenant.ChangeState(TenantState.Stopped);

            Assert.NotEqual(tenant.State, TenantState.Stopped);

            tenant.ChangeState(TenantState.Started);
            tenant.ChangeState(TenantState.Stopped);

            Assert.Equal(tenant.State, TenantState.Stopped);
        }

        [Fact]
        public void TenantIgnoresChangesStateWhenShutdownUntilBooted()
        {
            var tenant = new Tenant(new TenantConfiguration(new List<IConfigurationSource>()));
            tenant.Initialize("tenant1", Mock.Of<IServiceProvider>());

            tenant.ChangeState(TenantState.Shutdown);

            Assert.NotEqual(tenant.State, TenantState.Shutdown);

            tenant.ChangeState(TenantState.Boot);
            tenant.ChangeState(TenantState.Shutdown);

            Assert.Equal(tenant.State, TenantState.Shutdown);
        }
    }
}
