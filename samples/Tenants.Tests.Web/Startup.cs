using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Http;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.ServiceLookup;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;

namespace Tenants.Tests.Web
{
    class DefaultTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public TenantIdentificationResult IdentifyTenant(HttpContext context)
        {
            return TenantIdentificationResult.Passed;
        }

        public Task<TenantIdentificationResult> IdentifyTenantAsync([NotNull] HttpContext context)
        {
            return Task.FromResult(this.IdentifyTenant(context));
        }
    }

    class PathTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public TenantIdentificationResult IdentifyTenant([NotNull] HttpContext context)
        {
            if (context.Items.ContainsKey("TENANTNAME"))
            {
                var tenantId = (string)context.Items["TENANTNAME"];
                return new TenantIdentificationResult(tenantId, true, true);
            }

            if (context.Request.Path.HasValue && context.Request.Path.Value.Count(x => x == '/') > 0)
            {
                var path = context.Request.Path.Value.TrimStart('/');
                var name = path.Split('/')[0];
                if (!string.IsNullOrWhiteSpace(name) && name != "favicon.ico")
                {
                    context.Items["TENANTNAME"] = name;
                    return new TenantIdentificationResult(name, true, true);
                }
            }

            return TenantIdentificationResult.Failed;
        }

        public Task<TenantIdentificationResult> IdentifyTenantAsync([NotNull] HttpContext context)
        {
            return Task.FromResult(this.IdentifyTenant(context));
        }
    }

    public class Startup
    {
        private ITenantIdentificationStrategy _tenantIdentificationStrategy;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ITenantIdentificationStrategy), typeof(PathTenantIdentificationStrategy));
            services.AddTransient<ITenantComposer, TenantEventStoreComposer>();
            services.AddTenantOnlySingleton<TenantEventStore, TenantEventStore>();
            services.AddMvc();
            services.AddAssembly(this);
            services.AddMultitenancy();
            services.AddHttpMultitenancy();
            services.AddMultitenancyLogging();

            return new ContainerBuilder()
                .Populate(services)
                .BuildMultitenancy();
        }

        public void Configure(IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddConsole();

            _tenantIdentificationStrategy = app.ApplicationServices.GetService<ITenantIdentificationStrategy>();

            app.Map("/admin", x =>
            {
                // This is for request services, if this isn't set then you're going to have issues
                x.UseMultitenancyApplication();
                x.UseMvc();
            });

            app.Map("/events", x => x.UseMvc().UseMiddleware<EventsMiddleware>());
            app.UseRuntimeInfoPage("/runtimeinfo");


            app.MapWhen(IsTenantInPath, x =>
            {
                x.UseMultitenancy();
                //x.UseMvc();
                x.UseMiddleware<TenantTestMiddleware2>();
                x.UseMiddleware<TenantTestMiddleware>();
                x.UseMiddleware<TenantEventsMiddleware>();
            });

            app.UseWelcomePage();
        }

        public bool IsTenantInPath(HttpContext context)
        {
            var result = _tenantIdentificationStrategy.IdentifyTenant(context);
            if (result.Enabled && result.Success)
            {
                var path = new PathString(string.Format("/{0}", result.Id));

                PathString remainingPath;
                if (path.StartsWithSegments(path, out remainingPath))
                {
                    // Move the request path so it appears as if it was rooted like normal.
                    PathString pathBase = context.Request.PathBase;
                    context.Request.PathBase = pathBase + path;
                    context.Request.Path = remainingPath;

                    return true;
                }
            }
            return false;
        }
    }
}
