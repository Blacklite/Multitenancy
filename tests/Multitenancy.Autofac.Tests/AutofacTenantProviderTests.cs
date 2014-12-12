using System;
using Xunit;
using Moq;
using Autofac;
using Microsoft.Framework.DependencyInjection;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Autofac;
using Blacklite.Framework.Multitenancy.ConfigurationModel;
using System.Linq;

namespace Multitenancy.Autofac.Tests
{
    public class AutofacTenantProviderTests
    {
        [Fact]
        public void ProvidesTenants()
        {
            var tenant = new Mock<ITenant>();

            var childServiceProvider = new Mock<IServiceProvider>();
            childServiceProvider.Setup(x => x.GetService(typeof(ITenant))).Returns(tenant.Object);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(childServiceProvider.Object).As<IServiceProvider>();
            var lifetimeScopeContainer = builder.Build();

            var lifetimeScope = new Mock<ILifetimeScope>();
            lifetimeScope.Setup(x => x.BeginLifetimeScope(AutofacTenantProvider.Tag)).Returns(lifetimeScopeContainer);

            //.Setup(x => x.Resolve< IServiceProvider>()).Returns(;
            var tenantConfigurationService = new Mock<ITenantConfigurationService>();

            var provider = new AutofacTenantProvider(lifetimeScope.Object, tenantConfigurationService.Object);

            var result = provider.GetOrAdd("tenant1");

            lifetimeScope.Verify(x => x.BeginLifetimeScope(AutofacTenantProvider.Tag), Times.Once);
            tenant.Verify(x => x.Initialize("tenant1"), Times.Once);

            Assert.Equal(result.Tenant, tenant.Object);
            Assert.Equal(result.ServiceProvider, childServiceProvider.Object);
        }

        [Fact]
        public void ProvidesListOfTenantIds()
        {
            var tenant = new Mock<ITenant>();

            var childServiceProvider = new Mock<IServiceProvider>();
            childServiceProvider.Setup(x => x.GetService(typeof(ITenant))).Returns(tenant.Object);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(childServiceProvider.Object).As<IServiceProvider>();
            var lifetimeScopeContainer = builder.Build();

            var lifetimeScope = new Mock<ILifetimeScope>();
            lifetimeScope.Setup(x => x.BeginLifetimeScope(AutofacTenantProvider.Tag)).Returns(lifetimeScopeContainer);

            //.Setup(x => x.Resolve< IServiceProvider>()).Returns(;
            var tenantConfigurationService = new Mock<ITenantConfigurationService>();

            var provider = new AutofacTenantProvider(lifetimeScope.Object, tenantConfigurationService.Object);

            provider.GetOrAdd("tenant1");
            provider.GetOrAdd("tenant2");
            provider.GetOrAdd("tenant3");
            provider.GetOrAdd("tenant4");
            provider.GetOrAdd("tenant5");

            Assert.Contains("tenant1", provider.Tenants);
            Assert.Contains("tenant2", provider.Tenants);
            Assert.Contains("tenant3", provider.Tenants);
            Assert.Contains("tenant4", provider.Tenants);
            Assert.Contains("tenant5", provider.Tenants);
            Assert.Equal(5, provider.Tenants.Count());

        }
    }
}