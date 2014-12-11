using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Multitenancy.Tests.ConfigurationModel
{
    public class TenantConfigurationServiceTests
    {
        [Fact]
        public void ConfiguresDescriptions()
        {
            var service = new TenantConfigurationService(Enumerable.Empty< ITenantConfigurationDescriber>());

            service.Configure(new Tenant(new TenantConfiguration()));

            Assert.True(true);
        }
    }
}
