﻿using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.ConfigurationModel;
using System;
using Xunit;

namespace Multitenancy.Tests
{
    public class TenantTests
    {
        [Fact]
        public void IntializeRunsOnlyOnce()
        {
            var tenant = new Tenant(new TenantConfiguration());

            Assert.Null(tenant.Id);

            tenant.Initialize("tenant1");

            Assert.Equal(tenant.Id, "tenant1");

            tenant.Initialize("tenant2");

            Assert.NotEqual(tenant.Id, "tenant2");
        }

        [Fact]
        public void TenantHasNoStateWhenCreated()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            Assert.Equal(tenant.State, TenantState.None);
        }

        [Fact]
        public void TenantChangesStateWhenBooted()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            tenant.ChangeState(TenantState.Boot);

            Assert.Equal(tenant.State, TenantState.Boot);
        }

        [Fact]
        public void TenantChangesStateWhenStarted()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            tenant.ChangeState(TenantState.Started);

            Assert.Equal(tenant.State, TenantState.Started);
        }

        [Fact]
        public void TenantIgnoresChangesStateWhenStoppedUntilStarted()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            tenant.ChangeState(TenantState.Stopped);

            Assert.NotEqual(tenant.State, TenantState.Stopped);

            tenant.ChangeState(TenantState.Started);
            tenant.ChangeState(TenantState.Stopped);

            Assert.Equal(tenant.State, TenantState.Stopped);
        }

        [Fact]
        public void TenantIgnoresChangesStateWhenShutdownUntilBooted()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            tenant.ChangeState(TenantState.Shutdown);

            Assert.NotEqual(tenant.State, TenantState.Shutdown);

            tenant.ChangeState(TenantState.Boot);
            tenant.ChangeState(TenantState.Shutdown);

            Assert.Equal(tenant.State, TenantState.Shutdown);
        }
    }
}
