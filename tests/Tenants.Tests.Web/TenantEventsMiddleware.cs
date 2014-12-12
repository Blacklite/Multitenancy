using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.ApplicationEvents;
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
        private readonly IList<string> _events = new List<string>();
        private readonly IDictionary<string, IList<string>> _tenantEvents = new Dictionary<string, IList<string>>();

        public TenantEventsMiddleware(RequestDelegate next, IApplicationObservable observable)
        {
            _next = next;
            observable.Subscribe(x =>
            {
                IList<string> tenantEvents = null;
                if (!_tenantEvents.TryGetValue(x.Tenant, out tenantEvents))
                {
                    tenantEvents = new List<string>();
                    _tenantEvents.Add(x.Tenant, tenantEvents);
                }
                var value = JsonConvert.SerializeObject(x);
                tenantEvents.Add(value);
                _events.Add(value);
            });
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var tenant = httpContext.RequestServices.GetService<ITenant>();

            var sb = new StringBuilder();
            sb.AppendLine("--------------------------")
                .AppendLine("Events").AppendLine();
            foreach (var evt in _events)
            {
                sb.AppendLine(evt);
            }

            IList<string> tenantEvents = null;
            if (_tenantEvents.TryGetValue(tenant.Id, out tenantEvents))
            {
                sb.AppendLine("--------------------------")
                    .AppendLine("Tenant Events").AppendLine();

                foreach (var evt in tenantEvents)
                {
                    sb.AppendLine(evt);
                }
            }

            await httpContext.Response.WriteAsync(sb.ToString());
        }
    }
}
