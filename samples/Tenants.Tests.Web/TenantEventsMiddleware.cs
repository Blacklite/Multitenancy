using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Events;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Tenants.Tests.Web
{
    public class TenantEventsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, IList<string>> _tenantEvents = new Dictionary<string, IList<string>>();

        public TenantEventsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext, TenantEventStore store)
        {
            var sb = new StringBuilder();
            sb.AppendLine("--------------------------")
                .AppendLine("Tenant Events").AppendLine();

            foreach (var evt in store)
            {
                sb.AppendLine(evt);
            }


            await httpContext.Response.WriteAsync(sb.ToString());
        }
    }
}
