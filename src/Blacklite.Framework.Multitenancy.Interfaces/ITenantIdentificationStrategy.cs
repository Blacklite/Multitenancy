﻿using Microsoft.AspNet.Http;
using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    /// <summary>
    /// Defines a provider that determines the current tenant ID from
    /// execution context.
    /// </summary>
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface ITenantIdentificationStrategy
    {
        /// <summary>
        /// Attempts to identify the tenant from the current execution context.
        /// </summary>
        /// <param name="tenantId">
        /// The current tenant identifier.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the tenant could be identified; <see langword="false" />
        /// if not.
        /// </returns>
        bool TryIdentifyTenant([NotNull] HttpContext context, out string tenantId);
    }
}