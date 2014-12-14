using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public static class ApplicationEventsServices
    {
        public static IEnumerable<IServiceDescriptor> GetApplicationEvents(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.ApplicationOnlySingleton<ITenantComposer, ApplicationBroadcastComposer>();
            yield return describe.ApplicationOnlySingleton<IApplicationObservable, ApplicationObservable>();
            yield return describe.ApplicationOnlySingleton<IApplicationOrchestrator, ApplicationOrchestrator>();
        }
    }
}
