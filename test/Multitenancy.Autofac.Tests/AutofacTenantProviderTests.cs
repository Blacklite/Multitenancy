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
            var tenant = new Tenant(new TenantConfiguration());

            var childServiceProvider = new Mock<IServiceProvider>();
            childServiceProvider.Setup(x => x.GetService(typeof(ITenant))).Returns(tenant);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(childServiceProvider.Object).As<IServiceProvider>();
            var lifetimeScopeContainer = builder.Build();

            var lifetimeScope = new Mock<ILifetimeScope>();
            lifetimeScope.Setup(x => x.BeginLifetimeScope(AutofacTenantProvider.TenantTag)).Returns(lifetimeScopeContainer);

            var tenantConfigurationService = new Mock<ITenantConfigurationService>();

            var provider = new AutofacTenantProvider(lifetimeScope.Object, tenantConfigurationService.Object);

            var result = provider.GetOrAdd("tenant1");

            lifetimeScope.Verify(x => x.BeginLifetimeScope(AutofacTenantProvider.TenantTag), Times.Once);

            Assert.Equal(result.Tenant, tenant);
            Assert.Equal(result.ServiceProvider, childServiceProvider.Object);
        }
    }
}
