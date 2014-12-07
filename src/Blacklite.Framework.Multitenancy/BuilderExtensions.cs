using Autofac;
using Blacklite;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
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

        public static IApplicationBuilder AddMultitenancy([NotNull] this IApplicationBuilder app,
            ContainerBuilder builder,
            IEnumerable<IServiceDescriptor> descriptors)
        {
            builder.Populate(descriptors);
            return app;
        }
    }
}