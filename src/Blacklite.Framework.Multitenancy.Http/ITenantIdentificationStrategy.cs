﻿using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy.Http
{

    /// <summary>
    /// Defines a provider that determines the current tenant ID from
    /// execution context.
    /// </summary>
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
        ITenantIdentificationResult TryIdentifyTenant([NotNull] HttpContext context);

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
        Task<ITenantIdentificationResult> TryIdentifyTenantAsync([NotNull] HttpContext context);
    }
}
