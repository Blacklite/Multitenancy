using Blacklite;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy.Http;
using Microsoft.AspNet.Hosting.Internal;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy([NotNull] this IApplicationBuilder app, TenantMiddlewareOptions options = null)
        {
            if (options == null)
            {
                options = new TenantMiddlewareOptions()
                {
                    ThrowWhenFailed = true,
                    ThrowWhenOffline = true
                };
            }

            app.UseMiddleware<TenantMiddleware>(options);

            // This little hack makes it so that requests services are run after our tenant middleware, ensuring that the tenant middle first replaces application services
            var requestServices = new AutoRequestServicesStartupFilter();
            requestServices.Configure(app, (a) => { })(app);

            return app;
        }

        public static IApplicationBuilder UseMultitenancyApplication([NotNull] this IApplicationBuilder app)
        {
            // This little hack makes it so that requests services are run after our tenant middleware, ensuring that the tenant middle first replaces application services
            var requestServices = new AutoRequestServicesStartupFilter();
            requestServices.Configure(app, (a) => { })(app);

            return app;
        }
    }
}
