using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Blacklite.Framework.Multitenancy;
using Autofac;

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
            if (context.Request.Path.HasValue)
            {
                var path = context.Request.Path.Value.TrimStart('/');
                var name = path.Split('/')[0];
                if (name != null)
                {
                    tenantId = name;
                    return true;
                }
            }
            //context.Request.Path
            tenantId = "Default";
            return true;
        }
    }

    public class Startup
    {
        private IContainer Container;
        private IServiceCollection _services;
        private IServiceCollection _tenantServices = new ServiceCollection();

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

            app.UseMultitenancy(builder, _services);

            Container = builder.Build();
            app.ApplicationServices = Container.Resolve<IServiceProvider>();
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

            app.UseMvc();
            app.UseMiddleware<TenantTestMiddleware>();
        }
    }
}
