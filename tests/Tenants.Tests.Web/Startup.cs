﻿using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Blacklite.Framework.Multitenancy;
using Autofac;
using System.Threading.Tasks;
using System.Linq;

namespace Tenants.Tests.Web
{
    class DefaultTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public bool TryIdentifyTenant(HttpContext context, out string tenantId)
        {
            tenantId = "Default";
            return true;
        }
    }

    class PathTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        public bool TryIdentifyTenant(HttpContext context, out string tenantId)
        {
            if (context.Items.ContainsKey("TENANTNAME"))
            {
                tenantId = (string)context.Items["TENANTNAME"];
                return true;
            }

            if (context.Request.Path.HasValue && context.Request.Path.Value.Count(x => x == '/') > 0)
            {
                var path = context.Request.Path.Value.TrimStart('/');
                var name = path.Split('/')[0];
                if (!string.IsNullOrWhiteSpace(name) && name != "favicon.ico")
                {
                    context.Items["TENANTNAME"] = tenantId = name;
                    return true;
                }
            }
            //context.Request.Path
            tenantId = null;
            return false;
        }
    }

    public class Startup
    {
        private IContainer Container;
        private IServiceCollection _services;
        private IServiceCollection _tenantServices = new ServiceCollection();
        private ITenantIdentificationStrategy _tenantIdentificationStrategy;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ITenantIdentificationStrategy), typeof(PathTenantIdentificationStrategy));
            services.AddMvc();
            services.AddAssembly(this);

            _services = services;
        }

        public void Configure(IApplicationBuilder app)
        {
            var builder = new ContainerBuilder();
            // Create the container and use the default application services as a fallback

            app.AddMultitenancy(builder, _services);

            Container = builder.Build();
            app.ApplicationServices = Container.Resolve<IServiceProvider>();
            _tenantIdentificationStrategy = app.ApplicationServices.GetService<ITenantIdentificationStrategy>();
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

            app.Map("/admin", x =>
            {
                x.UseMvc();
            });

            app.MapWhen(IsTenantInPath, x =>
            {
                x.UseMultitenancy();
                x.UseMvc();
                x.UseMiddleware<TenantTestMiddleware2>();
                x.UseMiddleware<TenantTestMiddleware>();
            });

            app.UseWelcomePage();
            /*
            app.UseMultitenancy();
            app.UseMvc();
            app.UseMiddleware<TenantTestMiddleware2>();
            app.UseMiddleware<TenantTestMiddleware>();*/
        }

        public bool IsTenantInPath(HttpContext context)
        {
            string tenantId;
            if (_tenantIdentificationStrategy.TryIdentifyTenant(context, out tenantId))
            {
                var path = new PathString("/\{tenantId}");

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
