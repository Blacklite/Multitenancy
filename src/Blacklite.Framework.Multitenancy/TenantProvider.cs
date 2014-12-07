﻿using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantProvider
    {
        ITenantScope GetOrCreateTenant(string tenantId);
        void DisposeTenant(string tennatId);
    }
}
