using Blacklite.Framework.Events;
using Blacklite.Framework.GlobalEvents;
using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public static class BlackliteMultitenancyApplicationEventsServices
    {
        public static IEnumerable<ServiceDescriptor> GetApplicationEvents()
        {
            yield return describe.ApplicationOnlySingleton<IEventObservable<IApplicationEvent>, ApplicationObservable>();
            yield return describe.ApplicationOnlySingleton<IEventOrchestrator<IApplicationEvent>, ApplicationOrchestrator>();
            yield return describe.Transient<ITenantComposer, ApplicationBroadcastComposer>();
            yield return describe.Transient<IGlobalEventSource, ApplicationEventSource>();
        }
    }
}
