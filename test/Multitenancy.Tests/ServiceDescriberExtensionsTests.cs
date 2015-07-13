using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;
using Xunit;

namespace Multitenancy.Tests
{
    public class ServiceDescriberExtensionsTests
    {
        [Fact]
        public void TenantSingletonUsingTypes()
        {
            var result = TenantOnlyServiceDescriptor.Singleton(typeof(ITenant), typeof(Tenant));
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.Equal(result.ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), result);
        }

        [Fact]
        public void TenantSingletonUsingGenericTypes()
        {
            var result = TenantOnlyServiceDescriptor.Singleton<ITenant, Tenant>();
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.Equal(result.ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), result);
        }

        [Fact]
        public void TenantSingletonUsingFactory()
        {
            var result = TenantOnlyServiceDescriptor.Singleton(typeof(ITenant), x => x.GetService<Tenant>());
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), result);
        }

        [Fact]
        public void TenantSingletonUsingGenericFactory()
        {
            var result = TenantOnlyServiceDescriptor.Singleton<ITenant>(x => x.GetService<Tenant>());
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), result);
        }
    }
}
