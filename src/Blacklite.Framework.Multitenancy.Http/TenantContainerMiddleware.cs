﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Blacklite.Framework.Multitenancy
{
    public class TenantContainerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _services;
        private readonly ITenantIdentificationStrategy _tenantIdentificationStrategy;

        public TenantContainerMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider,
            ITenantIdentificationStrategy tenantIdentificationStrategy)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _next = next;
            _services = serviceProvider;
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.RequestServices != null)
            {
                throw new Exception("TODO: nested request container scope? this is probably a mistake on your part?");
            }

            string tenantId;
            if (!_tenantIdentificationStrategy.TryIdentifyTenant(httpContext, out tenantId))
                throw new Exception("Could not identify tenant!");

            using (var container = TenantServicesContainer.EnsureTenantServices(httpContext, _services, tenantId))
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}