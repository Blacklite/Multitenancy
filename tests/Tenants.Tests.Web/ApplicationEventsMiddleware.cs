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
    public class ApplicationEventsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IList<string> _events = new List<string>();

        public ApplicationEventsMiddleware(RequestDelegate next, IApplicationObservable observable)
        {
            _next = next;
            observable.Subscribe(x =>
            {
                var value = JsonConvert.SerializeObject(x);
                _events.Add(value);
            });
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var applicationServices = httpContext.RequestServices.GetService<IApplicationObservable>();

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
