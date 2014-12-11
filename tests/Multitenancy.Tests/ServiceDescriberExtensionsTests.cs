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
            var describer = new ServiceDescriber();
            var result = describer.TenantSingleton(typeof(ITenant), typeof(Tenant));
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.Equal(result.ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantServiceDescriptor), result);
        }

        [Fact]
        public void TenantSingletonUsingGenericTypes()
        {
            var describer = new ServiceDescriber();
            var result = describer.TenantSingleton<ITenant, Tenant>();
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.Equal(result.ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantServiceDescriptor), result);
        }

        [Fact]
        public void TenantSingletonUsingFactory()
        {
            var describer = new ServiceDescriber();
            var result = describer.TenantSingleton(typeof(ITenant), x => x.GetService<Tenant>());
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.IsType(typeof(TenantServiceDescriptor), result);
        }

        [Fact]
        public void TenantSingletonUsingGenericFactory()
        {
            var describer = new ServiceDescriber();
            var result = describer.TenantSingleton<ITenant>(x => x.GetService<Tenant>());
            Assert.Equal(result.ServiceType, typeof(ITenant));
            Assert.IsType(typeof(TenantServiceDescriptor), result);
        }
    }
}