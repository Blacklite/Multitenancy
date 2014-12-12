using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Multitenancy.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddTenantSingletonUsingTypes()
        {
            var collection = new ServiceCollection();
            collection.AddTenantOnlySingleton(typeof(Tenant));
            Assert.Equal(1, collection.Count());
            Assert.Equal(collection.First().ServiceType, typeof(Tenant));
            Assert.Equal(collection.First().ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), collection.First());

            collection = new ServiceCollection();
            collection.AddTenantOnlySingleton(typeof(ITenant), typeof(Tenant));
            Assert.Equal(1, collection.Count());
            Assert.Equal(collection.First().ServiceType, typeof(ITenant));
            Assert.Equal(collection.First().ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), collection.First());
        }

        [Fact]
        public void AddTenantSingletonUsingGenericTypes()
        {
            var collection = new ServiceCollection();
            collection.AddTenantOnlySingleton<Tenant>();
            Assert.Equal(1, collection.Count());
            Assert.Equal(collection.First().ServiceType, typeof(Tenant));
            Assert.Equal(collection.First().ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), collection.First());

            collection = new ServiceCollection();
            collection.AddTenantOnlySingleton<ITenant, Tenant>();
            Assert.Equal(1, collection.Count());
            Assert.Equal(collection.First().ServiceType, typeof(ITenant));
            Assert.Equal(collection.First().ImplementationType, typeof(Tenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), collection.First());
        }

        [Fact]
        public void AddTenantSingletonUsingFactory()
        {
            var collection = new ServiceCollection();
            collection.AddTenantOnlySingleton(typeof(ITenant), x => x.GetService<Tenant>());
            Assert.Equal(1, collection.Count());
            Assert.Equal(collection.First().ServiceType, typeof(ITenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), collection.First());
        }

        [Fact]
        public void AddTenantSingletonUsingGenericFactory()
        {
            var collection = new ServiceCollection();
            collection.AddTenantOnlySingleton<ITenant>(x => x.GetService<Tenant>());
            Assert.Equal(1, collection.Count());
            Assert.Equal(collection.First().ServiceType, typeof(ITenant));
            Assert.IsType(typeof(TenantOnlyServiceDescriptor), collection.First());
        }
    }
}