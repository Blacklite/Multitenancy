using Blacklite.Framework.Features;
using Blacklite.Framework.Features.Describers;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.Features.Describers
{
    public class MultitenancyFeatureDescriber : FeatureDescriber
    {
        public MultitenancyFeatureDescriber(TypeInfo typeInfo)
            : base(typeInfo)
        {
            IsApplicationScoped = TypeInfo.GetCustomAttributes<ApplicationOnlyAttribute>().Any();
            IsTenantScoped = TypeInfo.GetCustomAttributes<TenantOnlyAttribute>().Any();
        }

        public bool IsTenantScoped { get; }

        public bool IsApplicationScoped { get; }
    }
}
