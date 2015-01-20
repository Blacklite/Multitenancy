using Blacklite;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy([NotNull] this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantContainerMiddleware>();
            return app;
        }
    }
}
