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
using Blacklite.Framework.Events;

namespace Tenants.Tests.Web
{
    public class EventsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IList<string> _events = new List<string>();

        public EventsMiddleware(RequestDelegate next)
        {
            _next = next;
            MultitenancyEvents.Global.Subscribe(x =>
            {
                var value = JsonConvert.SerializeObject(x);
                _events.Add(value);
            });
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            var sb = new StringBuilder();
            sb.AppendLine("--------------------------")
                .AppendLine("Events").AppendLine();
            foreach (var evt in _events)
            {
                sb.AppendLine(evt);
            }

            await httpContext.Response.WriteAsync(sb.ToString());
        }
    }
}
