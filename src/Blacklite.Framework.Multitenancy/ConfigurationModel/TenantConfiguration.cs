﻿using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    public interface ITenantConfiguration : IConfiguration
    {

    }

    public class TenantConfiguration : Configuration, ITenantConfiguration
    {

    }
}