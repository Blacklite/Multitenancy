using Blacklite.Framework.Multitenancy;
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
        public void TenantChangesStateWhenBooted()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            tenant.ChangeState(TenantState.Boot);

            Assert.Equal(tenant.State, TenantState.Boot);
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
        public void TenantChangesStateWhenBooted()
        {
            var tenant = new Tenant(new TenantConfiguration());
            tenant.Initialize("tenant1");

            tenant.ChangeState(TenantState.Boot);

            Assert.Equal(tenant.State, TenantState.Boot);
        }
    }
}